namespace Masa.EShop.Services.Catalog.Infrastructure.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly CatalogDbContext _context;

    public CatalogItemRepository(CatalogDbContext context) => _context = context;

    public async Task AddAsync(CatalogItem catalogItem)
    {
        await _context.CatalogItems.AddAsync(catalogItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int catalogId)
    {
        var catalogItem = await _context.Set<CatalogItem>().FirstOrDefaultAsync(catalogType => catalogType.Id == catalogId) ?? throw new ArgumentNullException("CatalogItem does not exist");
        _context.CatalogItems.Remove(catalogItem);
        await _context.SaveChangesAsync();
    }

    public IQueryable<CatalogItem> Query(Expression<Func<CatalogItem, bool>> predicate)
    {
        return _context.Set<CatalogItem>().Where(predicate);
    }

    public async Task<CatalogItem> SingleAsync(int productionId)
    {
        return await _context.CatalogItems
            .Include(catalogItem => catalogItem.CatalogType)
            .Include(catalogItem => catalogItem.CatalogBrand)
            .AsSplitQuery()
            .SingleAsync(catalogItem => catalogItem.Id == productionId);
    }
}
