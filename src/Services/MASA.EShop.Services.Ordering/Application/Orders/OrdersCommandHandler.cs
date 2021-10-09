namespace MASA.EShop.Services.Ordering.Application.Orders
{
    public class OrdersCommandHandler
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        private async Task<IOrderingProcessActor> GetOrderingProcessActorAsync(int orderNumber)
        {
            var order = await _orderRepository.GetOrderByOrderNumberAsync(orderNumber);
            if (order == null)
            {
                throw new ArgumentException($"Order with order number {orderNumber} not found.");
            }

            var actorId = new ActorId(order.Id.ToString());
            return ActorProxy.Create<IOrderingProcessActor>(actorId, nameof(OrderingProcessActor));
        }

        [EventHandler]
        public async Task CancelOrderAsync(OrderCancelCommand command)
        {
            var orderingProcessActor = await GetOrderingProcessActorAsync(command.OrderNumber);
            await orderingProcessActor.Cancel();
        }

        [EventHandler]
        public async Task ShipOrderAsync(OrderShipCommand command)
        {
            var orderingProcessActor = await GetOrderingProcessActorAsync(command.OrderNumber);
            await orderingProcessActor.Ship();
        }
    }
}
