namespace Masa.EShop.Services.Catalog.Application.Catalogs;

public class ProductQueryHandler
{
    private readonly ICatalogItemRepository _catalogItemRepository;

    public ProductQueryHandler(ICatalogItemRepository catalogItemRepository)
        => _catalogItemRepository = catalogItemRepository;

    [EventHandler]
    public async Task ProductsHandleAsync(ProductsQuery query)
    {
        var _query = _catalogItemRepository.Query(a => (query.TypeId <= 0 || a.CatalogTypeId == query.TypeId)
                    && (query.BrandId <= 0 || a.CatalogBrandId == query.BrandId));

        var totalItems = await _query
            .LongCountAsync();

        var itemsOnPage = await _query
            .OrderBy(item => item.Name)
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        query.Result = new PaginatedItemsViewModel<CatalogItem>(query.PageIndex, query.PageSize, totalItems, itemsOnPage);
    }

    [EventHandler]
    public async Task ProductHandleAsync(ProductQuery query)
    {
        query.Result = await _catalogItemRepository.SingleAsync(query.ProductId);
    }
}

