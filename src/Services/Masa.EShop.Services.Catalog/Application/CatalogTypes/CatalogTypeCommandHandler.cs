namespace Masa.EShop.Services.Catalog.Application.CatalogTypes;

public class CatalogTypeCommandHandler
{
    private readonly ICatalogTypeRepository _catalogTypeRepository;

    public CatalogTypeCommandHandler(ICatalogTypeRepository catalogTypeRepository)
        => _catalogTypeRepository = catalogTypeRepository;

    [EventHandler]
    public async Task CreateHandleAsync(CreateCatalogTypeCommand @event)
        => await _catalogTypeRepository.AddAsync(new CatalogType(@event.Type));

    [EventHandler]
    public async Task DeleteHandlerAsync(DeleteCatalogTypeCommand command)
        => await _catalogTypeRepository.DeleteAsync(command.TypeId);
}
