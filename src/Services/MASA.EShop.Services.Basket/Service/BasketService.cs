namespace MASA.EShop.Services.Basket.Service;

public class BasketService : ServiceBase
{
    private const string DAPR_PUBSUB_NAME = "pubsub";

    private readonly ILogger<BasketService> _logger;

    public BasketService(
        IServiceCollection services,
        ILogger<BasketService> logger) : base(services)
    {
        _logger = logger;

        App.MapGet("/api/v1/basket/{id}", GetBasketByIdAsync).WithMetadata(new EndpointNameMetadata("get_basket"));
        App.MapPost("/api/v1/basket/updatebasket", UpdateBasketAsync);
        App.MapPost("/api/v1/basket/checkout", CheckoutAsync);
        App.MapDelete("/api/v1/basket/{id}", DeleteBasketByIdAsync);
        App.MapPost("/api/v1/basket/orderstarted", OrderStarted);
    }

    public async Task<IResult> GetBasketByIdAsync(string id, [FromServices] IBasketRepository repository)
    {
        var basket = await repository.GetBasketAsync(id);
        return Results.Ok(basket ?? new CustomerBasket(id));
    }

    public async Task<IResult> UpdateBasketAsync([FromBody] CustomerBasket value, [FromServices] IBasketRepository repository)
    {
        return Results.Ok(await repository.UpdateBasketAsync(value));
    }

    public async Task<IResult> CheckoutAsync([FromBody] BasketCheckout basketCheckout,
        [FromHeader(Name = "x-requestid")] string requestId,
        HttpContext httpContext,
        [FromServices] IIntegrationEventBus integrationEventBus,
        [FromServices] IntegrationEventLogContext logContext,
        [FromServices] IIntegrationEventLogService integrationEventLogService,
        [FromServices] IBasketRepository repository)
    {
        //todo update userId
        var userId = "user-id";

        basketCheckout.RequestId = (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty) ?
            guid : basketCheckout.RequestId;

        var basket = await repository.GetBasketAsync(userId);
        if (basket == null)
        {
            basket = new();
            //return Results.BadRequest();
        }
        //var userName = httpContext.User.FindFirst(x => x.Type == ClaimTypes.Name)?.Value ?? "";
        var userName = "userName";
        using (var transaction = logContext.Database.BeginTransaction())
        {
            var @event = new UserCheckoutAcceptedIntegrationEvent(
                userId,
                userName,
                basketCheckout.City,
                basketCheckout.Street,
                basketCheckout.State,
                basketCheckout.Country,
                basketCheckout.ZipCode,
                basketCheckout.CardNumber,
                basketCheckout.CardHolderName,
                basketCheckout.CardExpiration,
                basketCheckout.CardSecurityNumber,
                basketCheckout.CardTypeId,
                basketCheckout.Buyer,
                basketCheckout.RequestId,
                basket);
            await integrationEventLogService.SaveEventAsync(@event, transaction.GetDbTransaction());
            await integrationEventBus.PublishAsync(@event);
            transaction.Commit();
        }
        
        _logger.LogInformation("Publish Event CheckoutAccepted");

        return Results.Accepted();
    }

    public async Task DeleteBasketByIdAsync(string id, [FromServices] IBasketRepository repository)
    {
        await repository.DeleteBasketAsync(id);
    }

    [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToSubmittedIntegrationEvent))]
    public async Task OrderStarted(OrderStatusChangedToSubmittedIntegrationEvent @event, [FromServices] IBasketRepository repository)
    {
        _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, "Basket", @event);

        await repository.DeleteBasketAsync(@event.BuyerId);
    }
}
