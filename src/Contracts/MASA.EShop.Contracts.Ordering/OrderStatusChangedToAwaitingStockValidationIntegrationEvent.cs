using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;

namespace MASA.EShop.Contracts.Ordering
{
    public class OrderStatusChangedToAwaitingStockValidationIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; init; }
        public string OrderStatus { get; init; } = default!;
        public string Description { get; init; } = default!;
        public string BuyerName { get; init; } = default!;
        public IEnumerable<OrderStockItem> OrderStockItems { get; set; } = default!;
        public override string Topic { get; set; } = nameof(OrderStatusChangedToAwaitingStockValidationIntegrationEvent);

        private OrderStatusChangedToAwaitingStockValidationIntegrationEvent()
        {
        }

        public OrderStatusChangedToAwaitingStockValidationIntegrationEvent(Guid orderId, string orderStatus,
            string description, string buyerName, IEnumerable<OrderStockItem> orderStockItems)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
            Description = description;
            BuyerName = buyerName;
            OrderStockItems = orderStockItems;
        }
    }
}
