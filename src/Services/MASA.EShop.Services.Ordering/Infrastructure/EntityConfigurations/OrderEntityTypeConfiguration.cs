using Order = Masa.EShop.Services.Ordering.Entities.Order;

namespace Masa.EShop.Services.Ordering.Infrastructure.EntityConfigurations;

class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> orderConfiguration)
    {
        orderConfiguration.ToTable("orders", OrderingContext.DEFAULT_SCHEMA);

        orderConfiguration.HasKey(o => o.Id);

        orderConfiguration.HasAlternateKey(o => o.OrderNumber);

        orderConfiguration.Property(o => o.OrderNumber);//.UseHiLo("orderseq", OrderingContext.DEFAULT_SCHEMA);

        orderConfiguration
            .OwnsOne(o => o.Address, a =>
            {
                a.WithOwner();
            });
    }
}

