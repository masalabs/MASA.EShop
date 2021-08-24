
namespace MASA.EShop.Services.Catalog.Domain.Repositories.Interfaces;
public interface ICatalogTypeRepository
{
    Task<List<CatalogType>> Get(ISpecification<CatalogType> spec);

    Task CreateAsync(CatalogType catalog);
}
