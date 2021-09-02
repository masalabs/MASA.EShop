
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;

namespace MASA.EShop.Services.Catalog.Application.CatalogTypes.Commands;
public class CreateCatalogTypeCommandValidator : AbstractValidator<CreateCatalogTypeCommand>
{
    public CreateCatalogTypeCommandValidator()
    {
        RuleFor(cmd => cmd.Type)
            .NotNull().NotEmpty().WithMessage("Please input catalog type.")
            .Length(2, 20).WithMessage($"The length of '{{PropertyName}}' must be between 2 and 20 characters. You entered {{TotalLength}} character.  ");
    }
}
