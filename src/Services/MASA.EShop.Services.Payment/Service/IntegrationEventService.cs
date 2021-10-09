namespace MASA.EShop.Services.Payment.Service;

public class IntegrationEventService : ServiceBase
{
    private const string DAPR_PUBSUB_NAME = "pubsub";

    public IntegrationEventService(
        IServiceCollection services)
        : base(services)
    {
        App.MapGet("/api/v1/payment/HelloWorld", HelloWorld);
        App.MapPost("/api/v1/payment/OrderStatusChangedToValidated", OrderStatusChangedToValidatedAsync);
    }

    public string HelloWorld()
    {
        return "Hello World";
    }

    [Topic(DAPR_PUBSUB_NAME, "OrderStatusChangedToValidatedIntegrationEvent")]
    public async Task OrderStatusChangedToValidatedAsync(
        OrderStatusChangedToValidatedIntegrationEvent integrationEvent,
        [FromServices] IDomainEventBus domainEventBus,
        [FromServices] ILogger<IntegrationEventService> logger)
    {
        logger.LogInformation("----- integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", integrationEvent.Id, Program.AppName, integrationEvent);

        await domainEventBus.PublishAsync(
            new OrderStatusChangedToValidatedCommand(integrationEvent.Id, integrationEvent.CreationTime)
            {
                OrderId = integrationEvent.OrderId
            });
    }
}
