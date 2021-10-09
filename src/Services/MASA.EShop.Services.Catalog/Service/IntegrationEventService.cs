namespace MASA.EShop.Services.Catalog.Service;

public class IntegrationEventService : ServiceBase
{
    public const string DAPR_PUBSUB_NAME = "pubsub";

    public IntegrationEventService(IServiceCollection services) : base(services)
    {
        App.MapPost("/api/v1/catalog/orderstatuschangedtoawaitingstockvalidation", OrderStatusChangedToAwaitingStockValidationAsync);
        App.MapPost("/api/v1/catalog/orderstatuschangedtopaid", OrderStatusChangedToPaidAsync);
    }

    [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToAwaitingStockValidationIntegrationEvent))]
    public async Task OrderStatusChangedToAwaitingStockValidationAsync(
        OrderStatusChangedToAwaitingStockValidationIntegrationEvent @event,
        [FromServices] IEventBus eventBus)
    {
        await eventBus.PublishAsync(new StockValidationCommand(@event.OrderId, @event.OrderStockItems));
    }

    [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToPaidIntegrationEvent))]
    public async Task OrderStatusChangedToPaidAsync(
        OrderStatusChangedToPaidIntegrationEvent @event,
        [FromServices] IEventBus eventBus)
    {
        await eventBus.PublishAsync(new RemoveStockCommand(@event.OrderId, @event.OrderStockItems));
    }
}
