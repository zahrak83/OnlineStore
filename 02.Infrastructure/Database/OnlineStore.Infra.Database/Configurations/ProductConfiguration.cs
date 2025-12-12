using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Core.Entities;
using System.Reflection.Emit;

namespace OnlineStore.Infra.Database.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(x => x.Price)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(x => x.CategoryId);

            builder.HasMany(p => p.Images)
                .WithOne(i => i.Product)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Product { Id = 1, Title = "Laptop Asus ROG Strix G15", Description = "Gaming Laptop Ryzen 7, RTX 3060", Price = 52000, Stock = 10, CategoryId = 1 },
                new Product { Id = 2, Title = "Laptop Apple MacBook Air M1", Description = "Apple M1, 8GB RAM, 256GB SSD", Price = 45000, Stock = 12, CategoryId = 1 },
                new Product { Id = 3, Title = "Laptop Dell XPS 13", Description = "Core i7, 16GB RAM, 512GB SSD", Price = 58000, Stock = 8, CategoryId = 1 },
                new Product { Id = 4, Title = "Laptop HP Pavilion 15", Description = "Core i5, 8GB RAM, 256GB SSD", Price = 28000, Stock = 15, CategoryId = 1 },
                new Product { Id = 5, Title = "Laptop Lenovo Legion 5", Description = "Ryzen 7, RTX 3050", Price = 49000, Stock = 9, CategoryId = 1 },

                new Product { Id = 6, Title = "Samsung Galaxy S23", Description = "Flagship Snapdragon 8 Gen2", Price = 34000, Stock = 20, CategoryId = 2 },
                new Product { Id = 7, Title = "Apple iPhone 14", Description = "128GB, Super Retina XDR", Price = 42000, Stock = 17, CategoryId = 2 },
                new Product { Id = 8, Title = "Xiaomi Redmi Note 12", Description = "Great mid-range, 120Hz", Price = 11000, Stock = 30, CategoryId = 2 },
                new Product { Id = 9, Title = "Google Pixel 7", Description = "Tensor G2, Amazing Camera", Price = 28000, Stock = 12, CategoryId = 2 },
                new Product { Id = 10, Title = "Nokia X20", Description = "Reliable with clean Android", Price = 9000, Stock = 18, CategoryId = 2 },

                new Product { Id = 11, Title = "Logitech MX Master 3S", Description = "Ergonomic Wireless Mouse", Price = 3500, Stock = 22, CategoryId = 3 },
                new Product { Id = 12, Title = "Apple AirPods Pro 2", Description = "ANC, Spatial Audio", Price = 9500, Stock = 15, CategoryId = 3 },
                new Product { Id = 13, Title = "Anker PowerCore 20k", Description = "High-capacity Power Bank", Price = 1800, Stock = 25, CategoryId = 3 },
                new Product { Id = 14, Title = "Sony WH-CH710N Headphones", Description = "Noise Cancelling Wireless", Price = 3800, Stock = 14, CategoryId = 3 }
                );
        }
    }
}
