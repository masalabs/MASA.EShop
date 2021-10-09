namespace MASA.EShop.Services.Catalog.Application.Catalog;

public class CreateProductCommand : Command
{
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public int CatalogBrandId { get; set; }

    public int CatalogTypeId { get; set; }

    public decimal Price { get; set; }

    public string? PictureFileName { get; set; }
}
