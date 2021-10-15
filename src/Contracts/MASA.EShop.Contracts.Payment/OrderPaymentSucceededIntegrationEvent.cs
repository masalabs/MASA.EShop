namespace MASA.EShop.Contracts.Payment;

public record OrderPaymentSucceededIntegrationEvent(Guid OrderId) : IntegrationEvent
{
    public override string Topic { get; set; } = nameof(OrderPaymentSucceededIntegrationEvent);
}
