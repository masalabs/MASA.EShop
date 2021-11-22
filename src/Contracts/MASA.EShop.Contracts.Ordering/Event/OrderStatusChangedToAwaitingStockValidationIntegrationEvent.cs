namespace MASA.EShop.Contracts.Ordering.Event;

public record OrderStatusChangedToAwaitingStockValidationIntegrationEvent(Guid OrderId, string OrderStatus,
        string Description, string BuyerName, IEnumerable<OrderStockItem> OrderStockItems) : IntegrationEvent
{
    public override string Topic { get; set; } = nameof(OrderStatusChangedToAwaitingStockValidationIntegrationEvent);
}

