namespace Masa.EShop.Services.Catalog.Application.Catalogs.Queries;

public record ProductQuery : Query<CatalogItem>
{
    public int ProductId { get; set; } = default!;
    public override CatalogItem Result { get; set; } = new CatalogItem();
}
