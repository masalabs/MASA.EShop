
namespace MASA.EShop.Services.Payment.Infrastructure;
// todo MASA.Contrib.DDD.Infrastructure.DbContexts.EF.EFDbContext
// todo auto audit in EFDbContext
//public class PaymentContext : EFDbContext
public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
    {

    }

    public DbSet<Domain.Payments.Payment> Payments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // todo apply configuration
        //builder.ApplyConfiguration(new PaymentEntityTypeConfiguration());
    }
}
