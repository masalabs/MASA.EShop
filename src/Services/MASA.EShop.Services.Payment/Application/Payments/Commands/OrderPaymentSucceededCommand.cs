namespace MASA.EShop.Services.Payment.Application.Payments.Commands;
public class OrderPaymentSucceededCommand : IIntegrationDomainEvent
{
    public Guid Id => throw new NotImplementedException();

    public DateTime CreationTime => throw new NotImplementedException();

    public string Topic { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Guid OrderId { get; set; }

    public OrderPaymentSucceededCommand(Guid orderId)
    {
        OrderId = orderId;
    }
}
