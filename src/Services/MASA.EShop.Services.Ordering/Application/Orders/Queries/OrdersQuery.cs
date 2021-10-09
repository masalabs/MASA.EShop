namespace MASA.EShop.Services.Ordering.Application.Orders.Queries
{
    public class OrdersQuery : Query<IEnumerable<OrderSummary>>
    {
        public string ByuerId { get; set; }

        public override IEnumerable<OrderSummary> Result { get ; set ; }
    }
}
