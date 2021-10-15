using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;

namespace MASA.EShop.Contracts.Basket
{

    public record OrderStatusChangedToSubmittedIntegrationEvent(Guid OrderId, string OrderStatus,
            string BuyerId, string BuyerName) : IntegrationEvent
    {
        public override string Topic { get; set; } = nameof(OrderStatusChangedToSubmittedIntegrationEvent);
    }
}
