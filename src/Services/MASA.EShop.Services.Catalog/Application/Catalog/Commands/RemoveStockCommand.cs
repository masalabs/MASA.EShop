namespace MASA.EShop.Services.Catalog.Application.Catalog.Commands;

public class RemoveStockCommand : Command
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
