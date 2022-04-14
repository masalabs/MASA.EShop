namespace Masa.EShop.Services.Ordering.Application.Orders;

public class OrdersQueryHandler
{
    private readonly IOrderRepository _orderRepository;

    public OrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [EventHandler]
    public async Task OrderQueryAsync(OrderQuery query)
    {
        var order = await _orderRepository.GetOrderByOrderNumberAsync(query.OrderNumber);
        if (order is null)
        {
            //Service drop
            order = new Entities.Order();
        }
        query.Result = order;
    }

    [EventHandler]
    public async Task OrdersQueryAsync(OrdersQuery query)
    {
        query.Result = await _orderRepository.GetOrdersFromBuyerAsync(query.BuyerId);
    }
}

