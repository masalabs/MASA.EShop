
namespace MASA.EShop.Services.Catalog.Application.CatalogTypes;
public class CatalogTypeCommandHandler
{
    private readonly ICatalogTypeRepository _repository;

    public CatalogTypeCommandHandler(ICatalogTypeRepository repository)
    {
        _repository = repository;
    }

    // todo add dispatch handle attribute
    public async Task HandleAsync(CreateCatalogTypeCommand command)
    {
        CatalogType catalogType = new((int)DateTime.Now.Ticks, command.Type);

        await _repository.AddAsync(catalogType);
    }
}
