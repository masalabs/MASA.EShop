namespace MASA.EShop.Services.Payment.Application.Payments.Commands;

public class OrderPaymentSucceededDomainEvent : OrderPaymentSucceededIntegrationEvent, IIntegrationDomainEvent
{
    public OrderPaymentSucceededDomainEvent(Guid orderId) : base(orderId) { }
}
