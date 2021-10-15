namespace MASA.EShop.Services.Catalog.Application.Catalog.Commands;

public record RemoveStockCommand : Command
{
    public Guid OrderId { get; set; }

    public IEnumerable<OrderStockItem> OrderStockItems { get; set; }

    public RemoveStockCommand(Guid orderId,
        IEnumerable<OrderStockItem> orderStockItems)
    {
        OrderId = orderId;
        OrderStockItems = orderStockItems;
    }
}
