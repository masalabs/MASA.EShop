namespace MASA.EShop.Services.Catalog.Application.Catalog.Commands
{
    public record DeleteProductCommand : Command
    {
        public int ProductId { get; set; }
    }
}
