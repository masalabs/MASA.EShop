namespace MASA.EShop.Services.Ordering.Application.Orders.Commands;

public record AddOrderCommand(Entities.Order Order) : Command
{
    public Entities.Order Result { get; set; } = default!;
}

