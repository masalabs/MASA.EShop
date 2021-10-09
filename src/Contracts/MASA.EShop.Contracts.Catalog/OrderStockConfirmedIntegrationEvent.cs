namespace MASA.EShop.Contracts.Catalog;

public class OrderStockConfirmedIntegrationEvent : IntegrationEvent
{
    public override string Topic { get; set; } = nameof(OrderStockConfirmedIntegrationEvent);

    public Guid OrderId { get; init; }

    private OrderStockConfirmedIntegrationEvent()
    {
    }

    public OrderStockConfirmedIntegrationEvent(Guid orderId) => OrderId = orderId;
}
