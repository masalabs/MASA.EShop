namespace Masa.EShop.Services.Catalog.Infrastructure.Repositories;

public class CatalogTypeRepository : ICatalogTypeRepository
{
    private readonly CatalogDbContext _context;

    public CatalogTypeRepository(CatalogDbContext context) => _context = context;

    public async Task AddAsync(CatalogType catalogType)
    {
        await _context.CatalogTypes.AddAsync(catalogType);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int catalogTypeId)
    {
        var catalogType = await _context.Set<CatalogType>().FirstOrDefaultAsync(catalogType => catalogType.Id == catalogTypeId) ?? throw new ArgumentNullException("CatalogType does not exist");
        _context.CatalogTypes.Remove(catalogType);
        await _context.SaveChangesAsync();
    }

    public IQueryable<CatalogType> GetAll()
    {
        return _context.Set<CatalogType>().AsQueryable();
    }
}
