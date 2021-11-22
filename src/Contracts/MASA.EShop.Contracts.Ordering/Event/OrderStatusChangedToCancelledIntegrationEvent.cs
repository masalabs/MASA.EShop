namespace MASA.EShop.Contracts.Ordering.Event;

public record OrderStatusChangedToCancelledIntegrationEvent(Guid OrderId, string OrderStatus,
        string Description, string BuyerName) : IntegrationEvent
{
    public override string Topic { get; set; } = nameof(OrderStatusChangedToCancelledIntegrationEvent);
}

