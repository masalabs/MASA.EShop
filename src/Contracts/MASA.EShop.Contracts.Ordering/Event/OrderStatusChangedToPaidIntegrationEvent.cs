namespace MASA.EShop.Contracts.Ordering.Event;

public record OrderStatusChangedToPaidIntegrationEvent(Guid OrderId, string OrderStatus,
        string Description, string BuyerName, IEnumerable<OrderStockItem> OrderStockItems) : IntegrationEvent
{
    public override string Topic { get; set; } = nameof(OrderStatusChangedToPaidIntegrationEvent);
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

