namespace MASA.EShop.Services.Catalog.Dto;
public class CatalogItemDto
{
    public int id { get; set; }
    public string name { get; set; } = default!;
    public string description { get; set; } = default!;
    public decimal price { get; set; }
    public string pictureFileName { get; set; } = default!;
    public string type { get; set; } = default!;
    public string brand { get; set; } = default!;
    public int availableStock { get; set; }

    public static CatalogItemDto FromOrderItem(CatalogItem catalogItem)
    {
        return new CatalogItemDto
        {
            id = catalogItem.Id,
            name = catalogItem.Name,
            description = catalogItem.Description ?? "",
            price = catalogItem.Price,
            pictureFileName = catalogItem.PictureFileName,
            type = catalogItem.CatalogType.Type,
            brand = catalogItem.CatalogBrand.Brand,
            availableStock = catalogItem.AvailableStock
        };
    }
}

