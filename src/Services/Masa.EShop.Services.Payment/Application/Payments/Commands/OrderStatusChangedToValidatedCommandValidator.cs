namespace Masa.EShop.Services.Payment.Application.Payments.Commands;

public class OrderStatusChangedToValidatedCommandValidator : AbstractValidator<OrderStatusChangedToValidatedCommand>
{
    public OrderStatusChangedToValidatedCommandValidator()
    {
        RuleFor(cmd => cmd.Id).NotEqual(default(Guid)).WithMessage("wrong id");
        RuleFor(cmd => cmd.OrderId).NotEqual(default(Guid)).WithMessage("wrong order id");
        RuleFor(cmd => cmd.CreationTime).GreaterThanOrEqualTo(DateTime.UtcNow.AddMinutes(-5)).WithMessage("abnormal payment time");
        RuleFor(cmd => cmd.CreationTime).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("2 abnormal payment time");
    }
}
