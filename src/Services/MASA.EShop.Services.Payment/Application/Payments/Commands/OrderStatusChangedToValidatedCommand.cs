namespace MASA.EShop.Services.Payment.Application.Payments.Commands;

public record OrderStatusChangedToValidatedCommand : DomainCommand
{
    public Guid OrderId { get; set; }

    public OrderStatusChangedToValidatedCommand() : this(Guid.NewGuid(), DateTime.UtcNow)
    {
    }

    public OrderStatusChangedToValidatedCommand(Guid id, DateTime creationTime) : base(id, creationTime) { }
}
