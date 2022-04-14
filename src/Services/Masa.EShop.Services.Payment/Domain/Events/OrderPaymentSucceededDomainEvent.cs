namespace Masa.EShop.Services.Payment.Application.Payments.Commands;

public record OrderPaymentSucceededDomainEvent : OrderPaymentSucceededIntegrationEvent, IIntegrationDomainEvent
{
    public OrderPaymentSucceededDomainEvent(Guid orderId) : base(orderId) { }
}
