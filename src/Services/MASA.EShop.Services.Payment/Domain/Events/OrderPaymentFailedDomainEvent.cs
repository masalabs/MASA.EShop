namespace MASA.EShop.Services.Payment.Application.Payments.Commands;

public record OrderPaymentFailedDomainEvent : OrderPaymentFailedIntegrationEvent, IIntegrationDomainEvent
{
    public OrderPaymentFailedDomainEvent(Guid orderId) : base(orderId) { }
}
