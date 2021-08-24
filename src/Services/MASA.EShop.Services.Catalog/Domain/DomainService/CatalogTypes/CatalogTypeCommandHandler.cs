
using MASA.EShop.Services.Catalog.Domain.DomainService.CatalogTypes.Commands;

namespace MASA.EShop.Services.Catalog.Domain.DomainService.CatalogTypes;
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
        CatalogType catalogType = new()
        {
            Type = command.Type
        };

        await _repository.CreateAsync(catalogType);
    }
}
