namespace MASA.EShop.Services.Catalog.Application.Catalogs.Commands;

public class StockValidationCommandValidator : AbstractValidator<StockValidationCommand>
{
    public StockValidationCommandValidator()
    {
        RuleFor(cmd => cmd.OrderId).NotEqual(Guid.Empty).WithMessage("wrong order Id");
        //RuleFor(cmd => cmd.OrderStockItems).Must(cmd => cmd.Count() > 0).WithMessage("the product ID and quantity are incorrect");
        //RuleForEach(cmd => cmd.OrderStockItems).SetValidator(new OrderStockItemValidator());
    }
}
