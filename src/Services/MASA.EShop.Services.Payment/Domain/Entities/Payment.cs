namespace MASA.EShop.Services.Payment.Domain.Entities;
public class Payment : AuditAggregateRoot<Guid, Guid>
{
    public bool Succeeded { get; protected set; }

    public Guid OrderId { get; protected set; }

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
