namespace Masa.EShop.Services.Catalog.Domain.Repositories;

public interface ICatalogItemRepository
{
    IQueryable<CatalogItem> Query(Expression<Func<CatalogItem, bool>> predicate);

    Task AddAsync(CatalogItem catalogItem);

    Task DeleteAsync(int catalogId);

    Task<CatalogItem> SingleAsync(int productionId);
}
