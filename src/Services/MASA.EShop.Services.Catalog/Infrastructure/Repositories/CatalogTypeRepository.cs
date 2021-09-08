
namespace MASA.EShop.Services.Catalog.Infrastructure.Repositories;
public class CatalogTypeRepository : ICatalogTypeRepository
{
    private readonly CatalogDbContext _context;

    public CatalogTypeRepository(CatalogDbContext context) { _context = context; }

    public async Task CreateAsync(CatalogType catalogType)
    {
        await _context.CatalogTypes.AddAsync(catalogType);

        await _context.SaveChangesAsync();
    }
}
