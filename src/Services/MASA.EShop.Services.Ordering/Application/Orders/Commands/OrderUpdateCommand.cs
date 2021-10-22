namespace MASA.EShop.Services.Ordering.Application.Orders.Commands;

public record OrderUpdateCommand(Guid OrderId, string OrderStatus,
        string Description, string BuyerName) : Command
{
}

