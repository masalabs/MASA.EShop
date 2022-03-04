namespace Masa.EShop.Services.Catalog.Application.Catalogs.Commands.CreateProduct;

public class ProductCommandHandler
{
    private readonly ICatalogItemRepository _repository;
    private readonly IIntegrationEventBus _integrationEventBus;

    public ProductCommandHandler(ICatalogItemRepository repository, IIntegrationEventBus integrationEventBus)
    {
        _repository = repository;
        _integrationEventBus = integrationEventBus;
    }

    [EventHandler]
    public async Task CreateHandleAsync(CreateProductCommand command)
    {
        var catalogItem = new CatalogItem()
        {
            CatalogBrandId = command.CatalogBrandId,
            CatalogTypeId = command.CatalogTypeId,
            Description = command.Description,
            Name = command.Name,
            PictureFileName = command.PictureFileName ?? "default.png",
            Price = command.Price
        };
        await _repository.AddAsync(catalogItem);
    }

    [EventHandler]
    public async Task DeleteHandlerAsync(DeleteProductCommand command)
        => await _repository.DeleteAsync(command.ProductId);

    [EventHandler]
    public async Task StockValidationHandlerAsync(StockValidationCommand command)
    {
        var confirmedOrderStockItems = new List<ConfirmedOrderStockItem>();

        foreach (var orderStockItem in command.OrderStockItems)
        {
            var catalogItem = await _repository.SingleAsync(orderStockItem.ProductId);
            var hasStock = catalogItem.AvailableStock >= orderStockItem.Units;
            var confirmedOrderStockItem = new ConfirmedOrderStockItem(catalogItem.Id, hasStock);

            confirmedOrderStockItems.Add(confirmedOrderStockItem);
        }

        IntegrationEvent integrationEvent = confirmedOrderStockItems.Any(c => !c.HasStock)
            ? new OrderStockRejectedIntegrationEvent(command.OrderId, confirmedOrderStockItems)
            : new OrderStockConfirmedIntegrationEvent(command.OrderId);

        await _integrationEventBus.PublishAsync(integrationEvent);
    }

    [EventHandler]
    public async Task RemoveStockHandlerAsync(RemoveStockCommand command)
    {
        foreach (var orderStockItem in command.OrderStockItems)
        {
            var catalogItem = await _repository.SingleAsync(orderStockItem.ProductId);
            if (catalogItem.AvailableStock == 0)
            {
                throw new Exception($"Empty stock, product item {catalogItem.Name} is sold out");
            }
            int removed = Math.Min(orderStockItem.Units, catalogItem.AvailableStock);
            catalogItem.AvailableStock -= removed;
        }
    }
}
