namespace MASA.EShop.Services.Basket.Service;

public class BasketService : ServiceBase
{
    private const string DAPR_PUBSUB_NAME = "pubsub";

    private readonly IBasketRepository _repository;
    private readonly IIntegrationEventBus _eventBus = default!;
    private readonly ILogger<BasketService> _logger;

    public BasketService(
        IServiceCollection services,
        IBasketRepository repository,
        IIntegrationEventBus eventBus,
        ILogger<BasketService> logger) : base(services)
    {
        _repository = repository;
        _eventBus = eventBus;
        _logger = logger;

        App.MapGet("/api/v1/basket/{id}", GetBasketByIdAsync).WithMetadata(new EndpointNameMetadata("get_basket"));
        App.MapPost("/api/v1/basket/updatebasket", UpdateBasketAsync);
        App.MapPost("/api/v1/basket/checkout", CheckoutAsync);
        App.MapDelete("/api/v1/basket/{id}", DeleteBasketByIdAsync);
    }

    public async Task<IResult> GetBasketByIdAsync(string id)
    {
        var basket = await _repository.GetBasketAsync(id);
        return Results.Ok(basket ?? new CustomerBasket(id));
    }

    public async Task<IResult> UpdateBasketAsync([FromBody] CustomerBasket value)
    {
        return Results.Ok(await _repository.UpdateBasketAsync(value));
    }

    public async Task<IResult> CheckoutAsync([FromBody] BasketCheckout basketCheckout, [FromHeader(Name = "x-requestid")] string requestId, HttpContext httpContext)
    {
        var userId = "";

        basketCheckout.RequestId = (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty) ?
            guid : basketCheckout.RequestId;

        var basket = await _repository.GetBasketAsync(userId);
        if (basket == null)
        {
            return Results.BadRequest();
        }
        var userName = httpContext.User.FindFirst(x => x.Type == ClaimTypes.Name)?.Value??"";

        await _eventBus.PublishAsync(new CheckoutAcceptedIntegrationEvent(userId,
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
                basket));

        _logger.LogInformation("Publish Event CheckoutAccepted");

        return Results.Accepted();
    }

    public async Task DeleteBasketByIdAsync(string id)
    {
        await _repository.DeleteBasketAsync(id);
    }

    [Topic(DAPR_PUBSUB_NAME, "OrderStartedIntegrationEvent")]
    public async Task OrderStarted(OrderStartedIntegrationEvent @event)
    {
        /*var handler = _serviceProvider.GetRequiredService<OrderStatusChangedToSubmittedIntegrationEventHandler>();
        await handler.Handle(@event);*/

    }
}
