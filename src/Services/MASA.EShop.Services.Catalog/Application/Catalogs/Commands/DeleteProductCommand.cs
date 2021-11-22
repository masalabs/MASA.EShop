namespace MASA.EShop.Services.Catalog.Application.Catalogs.Commands;

public record DeleteProductCommand : Command
{
    public int ProductId { get; set; }
}

