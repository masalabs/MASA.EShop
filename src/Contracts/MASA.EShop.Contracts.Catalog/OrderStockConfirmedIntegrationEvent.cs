namespace MASA.EShop.Contracts.Catalog;

public record OrderStockConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent
{
    public override string Topic { get; set; } = nameof(OrderStockConfirmedIntegrationEvent);
}
