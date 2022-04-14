namespace Masa.EShop.Services.Catalogs.Application.CatalogBrands;

public class CatalogBrandQueryHandler
{
    private readonly ICatalogBrandRepository _catalogBrandRepository;

    public CatalogBrandQueryHandler(ICatalogBrandRepository catalogBrandRepository)
        => _catalogBrandRepository = catalogBrandRepository;

    [EventHandler]
    public async Task BrandsQueryHandleAsync(CatalogBrandsQuery query)
    {
        query.Result = await _catalogBrandRepository.GetAll().ToListAsync();
    }
}

