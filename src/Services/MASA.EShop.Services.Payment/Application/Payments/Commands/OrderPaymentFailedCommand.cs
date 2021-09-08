namespace MASA.EShop.Services.Payment.Application.Payments.Commands;
public class OrderPaymentFailedCommand : IIntegrationDomainEvent
{
    public Guid Id => throw new NotImplementedException();

    public DateTime CreationTime => throw new NotImplementedException();

    public string Topic { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Guid OrderId { get; set; }

    public OrderPaymentFailedCommand(Guid orderId)
    {
        OrderId = orderId;
    }
}
