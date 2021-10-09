using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;

namespace MASA.EShop.Contracts.Ordering
{
    public class OrderStatusChangedToShippedIntegrationEvent : IntegrationEvent
    {
        public override string Topic { get; set ; } = nameof(OrderStatusChangedToShippedIntegrationEvent);

        public Guid OrderId { get; init; }
        public string OrderStatus { get; init; } = default!;
        public string Description { get; init; } = default!;
        public string BuyerName { get; init; } = default!;

        private OrderStatusChangedToShippedIntegrationEvent()
        {
        }

        public OrderStatusChangedToShippedIntegrationEvent(Guid orderId, string orderStatus,
            string description, string buyerName)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
            Description = description;
            BuyerName = buyerName;
        }
    }
}
