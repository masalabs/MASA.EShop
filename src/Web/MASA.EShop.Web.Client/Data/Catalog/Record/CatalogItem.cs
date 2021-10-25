namespace MASA.EShop.Web.Client.Data.Catalog.Record;

public record CatalogItem
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public decimal Price { get; init; }
    public string PictureFileName { get; init; } = default!;

    public string GetPictureUrl(string basePath) => $"{basePath}/pics/{PictureFileName}";
}

