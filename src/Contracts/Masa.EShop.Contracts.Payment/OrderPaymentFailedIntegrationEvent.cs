namespace Masa.EShop.Contracts.Payment;

public record OrderPaymentFailedIntegrationEvent(Guid OrderId) : IntegrationEvent
{
    public override string Topic { get; set; } = nameof(OrderPaymentFailedIntegrationEvent);
}
