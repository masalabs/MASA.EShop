namespace Masa.EShop.Services.Catalog.Application.CatalogTypes;

public class CatalogTypeQueryHandler
{
    private readonly ICatalogTypeRepository _catalogTypeRepository;

    public CatalogTypeQueryHandler(ICatalogTypeRepository catalogTypeRepository)
        => _catalogTypeRepository = catalogTypeRepository;

    [EventHandler]
    public async Task TypesQueryHandleAsync(CatalogTypesQuery query)
    {
        query.Result = await _catalogTypeRepository.GetAll().ToListAsync();
    }
}
