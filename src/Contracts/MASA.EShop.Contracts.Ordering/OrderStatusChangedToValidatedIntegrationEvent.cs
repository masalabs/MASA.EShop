using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;

namespace MASA.EShop.Contracts.Ordering
{
    public class OrderStatusChangedToValidatedIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; init; }

        public string OrderStatus { get; init; } = default!;

        public string Description { get; init; } = default!;

        public string BuyerName { get; init; } = default!;

        public decimal Total { get; init; }

        public override string Topic { get ; set ; } = nameof(OrderStatusChangedToValidatedIntegrationEvent);

        private OrderStatusChangedToValidatedIntegrationEvent()
        {
            
        }

        public OrderStatusChangedToValidatedIntegrationEvent(Guid orderId, string orderStatus,
            string description, string buyerName, decimal total)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
            Description = description;
            BuyerName = buyerName;
            Total = total;
        }
    }
}
