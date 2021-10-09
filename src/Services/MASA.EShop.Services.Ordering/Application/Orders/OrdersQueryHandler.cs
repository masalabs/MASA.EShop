namespace MASA.EShop.Services.Ordering.Application.Orders
{
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
            query.Result = await _orderRepository.GetOrderByOrderNumberAsync(query.OrderNumber);
        }

        [EventHandler]
        public async Task OrdersQueryAsync(OrdersQuery query)
        {
            query.Result = await _orderRepository.GetOrdersFromBuyerAsync(query.ByuerId);
        }
    }
}
