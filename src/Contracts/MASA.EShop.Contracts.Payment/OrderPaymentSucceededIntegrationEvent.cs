namespace MASA.EShop.Contracts.Payment;

public class OrderPaymentSucceededIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }

    public override string Topic { get; set; } = nameof(OrderPaymentSucceededIntegrationEvent);

    private OrderPaymentSucceededIntegrationEvent()
    {
    }

    public OrderPaymentSucceededIntegrationEvent(Guid orderId) => OrderId = orderId;
}
