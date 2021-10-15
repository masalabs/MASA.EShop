using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;

namespace MASA.EShop.Contracts.Ordering
{
    public record OrderStatusChangedToValidatedIntegrationEvent(Guid OrderId, string OrderStatus,
            string Description, string BuyerName, decimal Total) : IntegrationEvent
    {
        public override string Topic { get; set; } = nameof(OrderStatusChangedToValidatedIntegrationEvent);

    }
}
