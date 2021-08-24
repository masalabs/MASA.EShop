
using Microsoft.EntityFrameworkCore;

namespace MASA.EShop.Services.Catalog.Infrastructure;
public class CatalogContext : DbContext
{
    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
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
