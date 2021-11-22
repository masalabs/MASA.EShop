namespace MASA.EShop.Contracts.Catalog.Event;

public record OrderStockRejectedIntegrationEvent(Guid OrderId,
        List<ConfirmedOrderStockItem> OrderStockItems) : IntegrationEvent
{
    public override string Topic { get; set; } = nameof(OrderStockRejectedIntegrationEvent);
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
