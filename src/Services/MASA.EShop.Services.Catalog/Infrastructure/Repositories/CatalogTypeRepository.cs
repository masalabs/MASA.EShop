
namespace MASA.EShop.Services.Catalog.Infrastructure.Repositories;
public class CatalogTypeRepository : ICatalogTypeRepository
{
    private readonly CatalogContext _context;

    public CatalogTypeRepository(CatalogContext context) { _context = context; }

    public Task<List<CatalogType>> Get(ISpecification<CatalogType> spec)
    {
        throw new NotImplementedException();
    }

    public async Task CreateAsync(CatalogType catalog)
    {
        _context.CatalogTypes.Add(catalog);
        await _context.SaveChangesAsync();
    }
}
