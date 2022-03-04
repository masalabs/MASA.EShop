namespace Masa.EShop.Services.Catalog.Application.Catalogs.Commands;

public record StockValidationCommand : Command
{
    public Guid OrderId { get; set; }

    public IEnumerable<OrderStockItem> OrderStockItems { get; set; }

    public StockValidationCommand(Guid orderId, IEnumerable<OrderStockItem> orderStockItems)
    {
        OrderId = orderId;
        OrderStockItems = orderStockItems;
    }
}
