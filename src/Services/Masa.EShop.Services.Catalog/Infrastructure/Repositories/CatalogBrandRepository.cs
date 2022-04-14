namespace Masa.EShop.Services.Catalog.Infrastructure.Repositories;

public class CatalogBrandRepository : ICatalogBrandRepository
{
    private readonly CatalogDbContext _context;

    public CatalogBrandRepository(CatalogDbContext context) => _context = context;

    public IQueryable<CatalogBrand> GetAll()
    {
        return _context.Set<CatalogBrand>().AsQueryable();
    }
}
