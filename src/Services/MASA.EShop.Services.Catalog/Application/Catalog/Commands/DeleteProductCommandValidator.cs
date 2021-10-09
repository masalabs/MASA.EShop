namespace MASA.EShop.Services.Catalog.Application.Catalog.Commands
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(cmd => cmd.ProductId).GreaterThan(0).WithMessage("Please enter the ProductId");
        }
    }
}
