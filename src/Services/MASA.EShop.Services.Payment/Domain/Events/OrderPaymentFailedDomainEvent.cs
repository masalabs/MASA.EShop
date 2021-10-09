namespace MASA.EShop.Services.Payment.Application.Payments.Commands;

public class OrderPaymentFailedDomainEvent : OrderPaymentFailedIntegrationEvent, IIntegrationDomainEvent
{
    public OrderPaymentFailedDomainEvent(Guid orderId) : base(orderId) { }
}
