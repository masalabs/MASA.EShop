namespace MASA.EShop.Services.Ordering.Application.Orders.Queries;

public record class OrdersQuery : Query<IEnumerable<OrderSummary>>
{
    public string BuyerId { get; set; } = default!;

    public override IEnumerable<OrderSummary> Result { get; set; } = default!;
}

