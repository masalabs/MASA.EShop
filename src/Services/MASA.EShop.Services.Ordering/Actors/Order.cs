namespace MASA.EShop.Services.Ordering.Actors;

public class Order
{
    public DateTime OrderDate { get; set; }
    public OrderStatus OrderStatus { get; set; } = default!;
    public string? Description { get; set; }
    public OrderAddress Address { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public List<OrderItem> OrderItems { get; set; } = default!;

    public decimal GetTotal()
    {
        var result = OrderItems.Sum(o => o.Units * o.UnitPrice);

        return result < 0 ? 0 : result;
    }
}

