namespace MASA.EShop.Contracts.Ordering.Event;

public record OrderStatusChangedToValidatedIntegrationEvent(Guid OrderId, string OrderStatus,
        string Description, string BuyerName, decimal Total) : IntegrationEvent
{
    public override string Topic { get; set; } = nameof(OrderStatusChangedToValidatedIntegrationEvent);

}

