using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;

namespace MASA.EShop.Contracts.Ordering
{
    public class OrderStatusChangedToPaidIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; init; }
        public string OrderStatus { get; init; } = default!;
        public string Description { get; init; } = default!;
        public string BuyerName { get; init; } = default!;
        public IEnumerable<OrderStockItem> OrderStockItems { get; set; } = default!;
        public override string Topic { get; set; } = nameof(OrderStatusChangedToPaidIntegrationEvent);

        private OrderStatusChangedToPaidIntegrationEvent()
        {
        }

        public OrderStatusChangedToPaidIntegrationEvent(Guid orderId, string orderStatus,
            string description, string buyerName, IEnumerable<OrderStockItem> orderStockItems)
        {
            OrderId = orderId;
            Description = description;
            OrderStatus = orderStatus;
            BuyerName = buyerName;
            OrderStockItems = orderStockItems;
        }
    }

    public class OrderStockItem
    {
        public int ProductId { get; set; }
        public int Units { get; set; }

        public OrderStockItem(int productId, int units)
        {
            ProductId = productId;
            Units = units;
        }
    }
}
