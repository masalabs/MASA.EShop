namespace MASA.EShop.Contracts.Ordering.Event;

public record OrderStatusChangedToSubmittedIntegrationEvent(Guid OrderId, string OrderStatus,
        string BuyerId, string BuyerName) : IntegrationEvent
{
    public override string Topic { get; set; } = nameof(OrderStatusChangedToSubmittedIntegrationEvent);
}

