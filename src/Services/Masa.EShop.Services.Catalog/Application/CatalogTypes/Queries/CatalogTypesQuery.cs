namespace Masa.EShop.Services.Catalog.Application.CatalogTypes.Queries;

public record CatalogTypesQuery : Query<IList<CatalogType>>
{
    public override IList<CatalogType> Result { get; set; } = new List<CatalogType>();

}
