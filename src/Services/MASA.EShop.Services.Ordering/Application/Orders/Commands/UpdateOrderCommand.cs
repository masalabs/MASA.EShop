namespace Masa.EShop.Services.Ordering.Application.Orders.Commands;

public record UpdateOrderCommand(Guid OrderId, string OrderStatus,
        string Description, string BuyerName) : Command
{
}

