namespace Masa.EShop.Services.Catalog.Domain.Entities;

public class CatalogItem
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string PictureFileName { get; set; } = "";

    public int CatalogTypeId { get; set; }

    public CatalogType CatalogType { get; private set; } = null!;

    public int CatalogBrandId { get; set; }

    public CatalogBrand CatalogBrand { get; private set; } = null!;

    public int AvailableStock { get; set; }

    public int RestockThreshold { get; set; }

    public int MaxStockThreshold { get; set; }

    public bool OnReorder { get; set; }
}
