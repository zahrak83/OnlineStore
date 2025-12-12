using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Core.Entities;

namespace OnlineStore.Infra.Database.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.HasData(
                new Category { Id = 1, Name = "Laptop", Description = "Laptop products" },
                new Category { Id = 2, Name = "Mobile", Description = "Mobile Products" },
                new Category { Id = 3, Name = "Accessories", Description = "Accessories Products" }
            );
        }
    }
}
