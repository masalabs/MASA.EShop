namespace Masa.EShop.Services.Catalog.Application.CatalogBrands.Queries;

public record CatalogBrandsQuery : Query<IList<CatalogBrand>>
{
    public override IList<CatalogBrand> Result { get; set; } = new List<CatalogBrand>();
}

