using Order = MASA.EShop.Services.Ordering.Entities.Order;

namespace MASA.EShop.Services.Ordering.Application.Orders.Queries
{
    public record class OrderQuery : Query<Order>
    {
        public int OrderNumber { get; set; }

        public string UserId { get; set; }

        public override Order Result { get; set; } = default!;
    }
}
