namespace MASA.EShop.Contracts.Catalog.Event;

public record OrderStockConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent
{
    public override string Topic { get; set; } = nameof(OrderStockConfirmedIntegrationEvent);
}
