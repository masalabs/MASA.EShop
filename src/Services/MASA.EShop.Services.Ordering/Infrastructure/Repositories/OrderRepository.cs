using Order = MASA.EShop.Services.Ordering.Entities.Order;

namespace MASA.EShop.Services.Ordering.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderingContext _orderingContext;

    public OrderRepository(OrderingContext orderingContext)
    {
        _orderingContext = orderingContext ?? throw new ArgumentNullException(nameof(orderingContext));
        _orderingContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public async Task<Order> AddOrGetOrderAsync(Order order)
    {
        _orderingContext.Add(order);

        try
        {
            await _orderingContext.SaveChangesAsync();
            return order;
        }
        catch (DbUpdateException ex) when ((ex.InnerException as SqlException)?.Number == 2627) //key repeat
        {
            return await GetOrderByIdAsync(order.Id);
        }
    }

    public async Task<IEnumerable<CardType>> GetCardTypesAsync()
    {
        return await _orderingContext.CardTypes.ToListAsync();
    }

    public async Task<Order> GetOrderByIdAsync(Guid orderId)
    {
        return await _orderingContext.Orders.Include(o => o.OrderItems)
            .SingleAsync(o => o.Id == orderId);
    }

    public async Task<Order?> GetOrderByOrderNumberAsync(int orderNumber)
    {
        return await _orderingContext.Orders.Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
    }

    public async Task<IEnumerable<OrderSummary>> GetOrdersFromBuyerAsync(string buyerId)
    {
        return await _orderingContext.Orders.Where(o => o.BuyerId == buyerId)
            .Include(o => o.OrderItems)
            .Select(o => new OrderSummary
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                OrderDate = o.OrderDate,
                OrderStatus = o.OrderStatus,
                Total = o.GetTotal()
            })
            .ToListAsync();
    }

    public async Task UpdateOrderAsync(Order order)
    {
        _orderingContext.Update(order);
        await _orderingContext.SaveChangesAsync();
    }
}

