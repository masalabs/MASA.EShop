namespace MASA.EShop.Services.Catalog.Infrastructure;
public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
    }
    public DbSet<CatalogItem> CatalogItems { get; set; } = null!;

    public DbSet<CatalogBrand> CatalogBrands { get; set; } = null!;

    public DbSet<CatalogType> CatalogTypes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
        //builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
        //builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
    }
}
