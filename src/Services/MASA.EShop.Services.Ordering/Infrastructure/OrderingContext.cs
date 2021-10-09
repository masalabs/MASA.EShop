using Order = MASA.EShop.Services.Ordering.Entities.Order;
using OrderItem = MASA.EShop.Services.Ordering.Entities.OrderItem;

namespace MASA.EShop.Services.Ordering.Infrastructure
{
    public class OrderingContext: IntegrationEventLogContext
    {
        public const string DEFAULT_SCHEMA = "ordering";

        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<OrderItem> OrderItems { get; set; } = default!;
        public DbSet<CardType> CardTypes { get; set; } = default!;

        public OrderingContext(MasaDbContextOptions<OrderingContext> options) : base(options)
        {
        }

        protected override void OnModelCreatingExecuting(ModelBuilder builder)
        {
            builder.Entity<Order>().OwnsOne(o => o.Address);

            builder.ApplyConfiguration(new OrderEntityTypeConfiguration());
            builder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
            builder.ApplyConfiguration(new CardTypeEntityTypeConfiguration());
            base.OnModelCreatingExecuting(builder);
        }
    }
}
