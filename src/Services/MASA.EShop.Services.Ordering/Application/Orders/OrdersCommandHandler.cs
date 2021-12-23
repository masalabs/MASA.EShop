namespace MASA.EShop.Services.Ordering.Application.Orders;

public class OrdersCommandHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IHubContext<NotificationsHub> _hubContext;

    public OrdersCommandHandler(IOrderRepository orderRepository, IHubContext<NotificationsHub> hubContext)
    {
        _orderRepository = orderRepository;
        _hubContext = hubContext;
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
    public async Task CancelOrderAsync(CancelOrderCommand command)
    {
        var orderingProcessActor = await GetOrderingProcessActorAsync(command.OrderNumber);
        await orderingProcessActor.Cancel();
    }

    [EventHandler]
    public async Task AddOrderAsync(AddOrderCommand command)
    {
        if (command.Order.OrderNumber == 0)
        {
            //Generate OrderNumber
            command.Order.OrderNumber = int.Parse(DateTimeOffset.Now.ToString("HHmmssfff"));
        }
        command.Result = await _orderRepository.AddOrGetOrderAsync(command.Order);
    }

    [EventHandler]
    public async Task ShipOrderAsync(ShipOrderCommand command)
    {
        var orderingProcessActor = await GetOrderingProcessActorAsync(command.OrderNumber);
        await orderingProcessActor.Ship();
    }

    [EventHandler]
    public async Task UpdateOrderAsync(UpdateOrderCommand command)
    {
        var order = await _orderRepository.GetOrderByIdAsync(command.OrderId);
        if (order != null)
        {
            order.OrderStatus = command.OrderStatus;
            order.Description = command.Description;

            await _orderRepository.UpdateOrderAsync(order);
            await SendNotificationAsync(order.OrderNumber, command.OrderStatus, command.BuyerName);
        }
    }

    private Task SendNotificationAsync(int orderNumber, string orderStatus, string buyerName)
    {
        return _hubContext.Clients
            .Group(buyerName)
            .SendAsync("UpdatedOrderState", new { OrderNumber = orderNumber, Status = orderStatus });
    }
}

