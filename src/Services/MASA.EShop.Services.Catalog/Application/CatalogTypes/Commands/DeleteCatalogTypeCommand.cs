namespace Masa.EShop.Services.Catalog.Application.CatalogTypes.Commands.CreateCatalogType;

public record DeleteCatalogTypeCommand : Command
{
    public int TypeId { get; set; }
}
