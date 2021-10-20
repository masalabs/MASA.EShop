using MASA.EShop.Web.Client.Data.Ordering.Record;

namespace MASA.EShop.Web.Client.Data.Ordering
{
    public interface IOrderService
    {
        Task<List<OrderSummary>> GetMyOrders(string userId);
        Task<Order> GetOrder(string userId, int orderNumber);
        Task CancelOrder(int orderNumber);
        Task ShipOrder(int orderNumber);
    }
}
