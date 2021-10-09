namespace MASA.EShop.Services.Ordering.Application.Orders.Commands
{
    public class OrderCancelCommand : Command
    {
        public int OrderNumber { get; set; }
    }
}
