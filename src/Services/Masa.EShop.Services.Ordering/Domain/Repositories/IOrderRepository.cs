using Order = Masa.EShop.Services.Ordering.Entities.Order;

namespace Masa.EShop.Services.Ordering.Domain.Repositories;

public interface IOrderRepository
{
    Task<Order> GetOrderByIdAsync(Guid orderId);
    Task<Order?> GetOrderByOrderNumberAsync(int orderNumber);
    Task<Order> AddOrGetOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task<IEnumerable<OrderSummary>> GetOrdersFromBuyerAsync(string buyerId);
    Task<IEnumerable<CardType>> GetCardTypesAsync();
}

