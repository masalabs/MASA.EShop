namespace MASA.EShop.Services.Ordering.Actors
{
    public class Order
    {
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string Description { get; set; }
        public OrderAddress Address { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public decimal GetTotal()
        {
            var result = OrderItems.Sum(o => o.Units * o.UnitPrice);

            return result < 0 ? 0 : result;
        }
    }
}