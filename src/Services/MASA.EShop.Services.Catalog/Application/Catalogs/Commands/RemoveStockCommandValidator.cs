namespace MASA.EShop.Services.Catalog.Application.Catalogs.Commands;

public class RemoveStockCommandValidator : AbstractValidator<RemoveStockCommand>
{
    public RemoveStockCommandValidator()
    {
        RuleFor(cmd => cmd.OrderId).NotEqual(default(Guid)).WithMessage("Order error");
        //RuleFor(cmd => cmd.OrderStockItems).Must(cmd => cmd.Count() > 0).WithMessage("the product ID and quantity are incorrect");
        //RuleForEach(cmd => cmd.OrderStockItems).SetValidator(new OrderStockItemValidator());
    }
}

public class OrderStockItemValidator : AbstractValidator<OrderStockItem>
{
    public OrderStockItemValidator()
    {
        RuleFor(cmd => cmd.ProductId).GreaterThan(0).WithMessage("Product does not exist");
        RuleFor(cmd => cmd.Units).GreaterThan(0).WithMessage("Item units desired should be greater than zero");
    }
}
