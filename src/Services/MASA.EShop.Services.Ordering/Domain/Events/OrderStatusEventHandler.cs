namespace MASA.EShop.Services.Ordering.Domain.Events;

public class OrderStatusEventHandler
{
    private readonly IEventBus _eventBus;

    public OrderStatusEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    [EventHandler]
    public async Task OrderStatusChangedToSubmitted(OrderStatusChangedToSubmittedEvent integrationEvent)
    {
        await _eventBus.PublishAsync(new UpdateOrderCommand(integrationEvent.OrderId,
            integrationEvent.OrderStatus, "", integrationEvent.BuyerName));
    }

    [EventHandler]
    public async Task OrderStatusChangedToPaid(OrderStatusChangedToPaidIntegrationEvent integrationEvent)
    {
        await _eventBus.PublishAsync(new UpdateOrderCommand(integrationEvent.OrderId,
            integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerName));
    }

    [EventHandler]
    public async Task OrderStatusChangedToShipped(OrderStatusChangedToShippedEvent integrationEvent)
    {
        await _eventBus.PublishAsync(new UpdateOrderCommand(integrationEvent.OrderId,
            integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerName));
    }

    [EventHandler]
    public async Task OrderStatusChangedToCancelled(OrderStatusChangedToCancelledEvent integrationEvent)
    {
        await _eventBus.PublishAsync(new UpdateOrderCommand(integrationEvent.OrderId,
            integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerName));
    }



    [EventHandler]
    public async Task OrderStatusChangedToAwaitingStockValidation(OrderStatusChangedToAwaitingStockValidationEvent integrationEvent)
    {
        await _eventBus.PublishAsync(new UpdateOrderCommand(integrationEvent.OrderId,
            integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerName));
    }

    [EventHandler]
    public async Task OrderStatusChangedToValidated(OrderStatusChangedToValidatedIntegrationEvent integrationEvent)
    {
        await _eventBus.PublishAsync(new UpdateOrderCommand(integrationEvent.OrderId,
            integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerName));
    }
}
