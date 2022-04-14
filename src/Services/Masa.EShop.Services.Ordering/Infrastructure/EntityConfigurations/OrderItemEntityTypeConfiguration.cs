using OrderItem = Masa.EShop.Services.Ordering.Entities.OrderItem;

namespace Masa.EShop.Services.Ordering.Infrastructure.EntityConfigurations;

class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> orderItemConfiguration)
    {
        orderItemConfiguration.ToTable("orderItems", OrderingContext.DEFAULT_SCHEMA);

        orderItemConfiguration.HasKey(o => o.Id);

        orderItemConfiguration.Property(o => o.Id);//.UseHiLo("orderitemseq");
    }
}

