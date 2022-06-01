using Order = Masa.EShop.Services.Ordering.Entities.Order;

namespace Masa.EShop.Services.Ordering.Application.Orders.Queries;

public record class OrderQuery : Query<Order>
{
    public int OrderNumber { get; set; }

    public string UserId { get; set; } = default!;

    public override Order Result { get; set; } = default!;
}

