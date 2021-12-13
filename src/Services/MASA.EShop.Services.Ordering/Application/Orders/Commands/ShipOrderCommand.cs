namespace MASA.EShop.Services.Ordering.Application.Orders.Commands;

public record ShipOrderCommand : Command
{
    public int OrderNumber { get; set; }
}

