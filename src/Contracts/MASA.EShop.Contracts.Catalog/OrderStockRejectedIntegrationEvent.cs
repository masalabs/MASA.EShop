namespace MASA.EShop.Contracts.Catalog;

public class OrderStockRejectedIntegrationEvent : IntegrationEvent
{
    public override string Topic { get; set; } = nameof(OrderStockRejectedIntegrationEvent);

    public Guid OrderId { get; init; }

    public List<ConfirmedOrderStockItem> OrderStockItems { get; init; } = new();

    private OrderStockRejectedIntegrationEvent()
    {
    }

    public OrderStockRejectedIntegrationEvent(Guid orderId,
        List<ConfirmedOrderStockItem> orderStockItems)
    {
        OrderId = orderId;
        OrderStockItems = orderStockItems;
    }
}

public class ConfirmedOrderStockItem
{
    public int ProductId { get; set; }
    public bool HasStock { get; set; }

    public ConfirmedOrderStockItem(int productId, bool hasStock)
    {
        ProductId = productId;
        HasStock = hasStock;
    }
}
