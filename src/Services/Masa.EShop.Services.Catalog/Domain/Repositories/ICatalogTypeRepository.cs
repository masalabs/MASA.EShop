namespace Masa.EShop.Services.Catalog.Domain.Repositories;

public interface ICatalogTypeRepository
{
    Task AddAsync(CatalogType catalogType);

    Task DeleteAsync(int catalogTypeId);

    IQueryable<CatalogType> GetAll();
}
