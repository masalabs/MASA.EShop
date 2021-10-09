namespace MASA.EShop.Services.Catalog.Domain.Repositories;

public interface ICatalogItemRepository
{
    Task AddAsync(CatalogItem catalogItem);

    Task DeleteAsync(int catalogId);

    Task<CatalogItem> SingleAsync(int productionId);
}
