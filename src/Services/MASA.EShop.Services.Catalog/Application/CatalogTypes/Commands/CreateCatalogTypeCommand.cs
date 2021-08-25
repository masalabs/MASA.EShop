
namespace MASA.EShop.Services.Catalog.Application.CatalogTypes.Commands;
public class CreateCatalogTypeCommand : ICommand // todo add MASA.Contrib.ReadWriteSpliting.CQRS and replace to Command
{
    public string Type { get; set; } = null!;

    public Guid Id => Guid.NewGuid(); // todo remove

    public DateTime CreationTime => DateTime.Now; // todo remove
}
