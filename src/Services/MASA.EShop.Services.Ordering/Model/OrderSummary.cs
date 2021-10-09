namespace MASA.EShop.Services.Ordering.Model
{
    public class OrderSummary
    {
        public Guid Id { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; } = default!;
        public decimal Total { get; set; }
    }
}
