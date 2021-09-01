
namespace MASA.EShop.Services.Catalog.Application.CatalogTypes.Commands;
public class CreateCatalogTypeCommandValidator : AbstractValidator<CreateCatalogTypeCommand>
{
    public CreateCatalogTypeCommandValidator()
    {
        RuleFor(cmd => cmd.Type)
            .NotNull().NotEmpty().WithMessage("Please input catalog type.")
            .Length(2, 20);
    }
}
