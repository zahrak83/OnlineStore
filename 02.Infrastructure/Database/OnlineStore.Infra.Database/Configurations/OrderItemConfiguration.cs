using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Core.Entities;

namespace OnlineStore.Infra.Database.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(x => x.UnitPrice)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(x => x.OrderId);

            builder.HasOne(x => x.Product)
                   .WithMany()
                   .HasForeignKey(x => x.ProductId);
        }
    }
}
