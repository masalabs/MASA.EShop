namespace Masa.EShop.Services.Ordering.Domain.Events;

public record OrderStatusChangedToSubmittedEvent(Guid OrderId, string OrderStatus,
        string BuyerId, string BuyerName) : Event;

public record OrderStatusChangedToShippedEvent(Guid OrderId, string OrderStatus,
        string Description, string BuyerName) : Event;

public record OrderStatusChangedToCancelledEvent(Guid OrderId, string OrderStatus,
        string Description, string BuyerName) : Event;

public record OrderStatusChangedToAwaitingStockValidationEvent(Guid OrderId, string OrderStatus,
        string Description, string BuyerName, IEnumerable<OrderStockItem> OrderStockItems) : Event;

public record OrderStatusChangedToValidatedEvent(Guid OrderId, string OrderStatus,
        string Description, string BuyerName, decimal Total) : Event;

