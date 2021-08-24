
namespace MASA.EShop.Services.Catalog.Domain.DomainService.CatalogTypes.Commands;
public class CreateCatalogTypeCommandValidator : AbstractValidator<CreateCatalogTypeCommand>
{
    public CreateCatalogTypeCommandValidator()
    {
        RuleFor(x => x.Type)
            .NotNull().NotEmpty().WithMessage("Please input name.")
            .Length(2, 20);
    }
}
