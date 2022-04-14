namespace Masa.EShop.Services.Catalog.Application.Catalogs.Commands;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(cmd => cmd.ProductId).GreaterThan(0).WithMessage("Please enter the ProductId");
    }
}

