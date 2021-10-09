using Order = MASA.EShop.Services.Ordering.Entities.Order;

namespace MASA.EShop.Services.Ordering.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<Order?> GetOrderByOrderNumberAsync(int orderNumber);
        Task<Order> AddOrGetOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task<IEnumerable<OrderSummary>> GetOrdersFromBuyerAsync(string buyerId);
        Task<IEnumerable<CardType>> GetCardTypesAsync();
    }
}
