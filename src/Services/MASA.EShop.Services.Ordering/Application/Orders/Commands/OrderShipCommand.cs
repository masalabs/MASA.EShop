namespace MASA.EShop.Services.Ordering.Application.Orders.Commands;

public record OrderShipCommand : Command
{
    public int OrderNumber { get; set; }
}

