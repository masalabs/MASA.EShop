namespace MASA.EShop.Services.Ordering.Application.Orders.Commands;

public record CancelOrderCommand : Command
{
    public int OrderNumber { get; set; }
}

