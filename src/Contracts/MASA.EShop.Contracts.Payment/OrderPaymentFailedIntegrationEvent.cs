namespace MASA.EShop.Contracts.Payment;

public class OrderPaymentFailedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }

    public override string Topic { get; set; } = nameof(OrderPaymentFailedIntegrationEvent);

    private OrderPaymentFailedIntegrationEvent()
    {
    }

    public OrderPaymentFailedIntegrationEvent(Guid orderId) => OrderId = orderId;
}
