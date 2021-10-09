using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;

namespace MASA.EShop.Contracts.Basket
{
    public class OrderStatusChangedToSubmittedIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; init; }
        public string OrderStatus { get; init; } = default!;
        public string BuyerId { get; init; } = default!;
        public string BuyerName { get; init; } = default!;
        public override string Topic { get ; set ; } = nameof(OrderStatusChangedToSubmittedIntegrationEvent);

        private OrderStatusChangedToSubmittedIntegrationEvent()
        {
        }

        public OrderStatusChangedToSubmittedIntegrationEvent(Guid orderId, string orderStatus,
            string buyerId, string buyerName)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
            BuyerId = buyerId;
            BuyerName = buyerName;
        }
    }
}
