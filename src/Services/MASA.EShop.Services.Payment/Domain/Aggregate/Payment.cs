namespace MASA.EShop.Services.Payment.Domain.Aggregate;

public class Payment : AuditAggregateRoot<Guid, Guid>
{
    public bool Succeeded { get; protected set; }

    public Guid OrderId { get; protected set; }

    public Payment() { }

    public Payment(Guid orderId, bool succeeded)
    {
        OrderId = orderId;
        Succeeded = succeeded;
    }

    public void SetSucceeded(bool succeeded)
    {
        Succeeded = succeeded;
    }
}
