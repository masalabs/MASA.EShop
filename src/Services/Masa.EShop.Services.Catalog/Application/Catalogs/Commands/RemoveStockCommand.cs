namespace Masa.EShop.Services.Catalog.Application.Catalogs.Commands;

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
