namespace MASA.EShop.Services.Catalog.Application.CatalogTypes.Commands.CreateCatalogType;

public class CreateCatalogTypeCommand : Command
{
    public string Type { get; set; } = null!;
}
