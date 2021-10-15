namespace MASA.EShop.Web.Client.Data.Catalog.Record
{
    public record CatalogItem
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public string Description { get; init; } = default!;
        public decimal Price { get; init; }
        public string PictureUri { get; init; } = default!;
        public int CatalogBrandId { get; init; }
        public string CatalogBrand { get; init; } = default!;
        public int CatalogTypeId { get; init; }
        public string CatalogType { get; init; } = default!;
    }
}
