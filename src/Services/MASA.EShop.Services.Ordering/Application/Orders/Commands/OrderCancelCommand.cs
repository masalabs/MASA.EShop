namespace MASA.EShop.Services.Ordering.Application.Orders.Commands
{
    public record OrderCancelCommand : Command
    {
        public int OrderNumber { get; set; }
    }
}
