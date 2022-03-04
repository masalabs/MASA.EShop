namespace Masa.EShop.Services.Payment.Infrastructure;

public class PaymentDbContext : IntegrationEventLogContext
{
    public DbSet<Domain.Aggregate.Payment> Payments { get; set; } = null!;

    public PaymentDbContext(MasaDbContextOptions<PaymentDbContext> options) : base(options)
    {

    }
}
