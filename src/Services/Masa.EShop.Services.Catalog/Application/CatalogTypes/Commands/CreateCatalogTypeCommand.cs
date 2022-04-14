namespace Masa.EShop.Services.Catalog.Application.CatalogTypes.Commands.CreateCatalogType;

public record CreateCatalogTypeCommand : Command
{
    public string Type { get; set; } = null!;
}
