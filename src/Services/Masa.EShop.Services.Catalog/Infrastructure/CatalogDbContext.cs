namespace Masa.EShop.Services.Catalog.Infrastructure;

public class CatalogDbContext : MasaDbContext
{
    public DbSet<CatalogItem> CatalogItems { get; set; } = null!;

    public DbSet<CatalogBrand> CatalogBrands { get; set; } = null!;

    public DbSet<CatalogType> CatalogTypes { get; set; } = null!;

    public CatalogDbContext(MasaDbContextOptions<CatalogDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreatingExecuting(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
        builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
        builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
        base.OnModelCreatingExecuting(builder);
    }
}
