using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Core.Entities;

namespace OnlineStore.Infra.Database.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasData(
                new Image { Id = 1, ProductId = 1, FileName = "asus_g15.jpg", FilePath = "/images/products/asus_g15.jpg" },
                new Image { Id = 2, ProductId = 2, FileName = "macbook_air_m1.jpg", FilePath = "/images/products/macbook_air_m1.jpg" },
                new Image { Id = 3, ProductId = 3, FileName = "dell_xps13.jpg", FilePath = "/images/products/dell_xps13.jpg" },
                new Image { Id = 4, ProductId = 4, FileName = "hp_pavilion15.jpg", FilePath = "/images/products/hp_pavilion15.jpg" },
                new Image { Id = 5, ProductId = 5, FileName = "lenovo_legion5.jpg", FilePath = "/images/products/lenovo_legion5.jpg" },

                new Image { Id = 6, ProductId = 6, FileName = "samsung_s23.jpg", FilePath = "/images/products/samsung_s23.jpg" },
                new Image { Id = 7, ProductId = 7, FileName = "iphone14.jpg", FilePath = "/images/products/iphone14.jpg" },
                new Image { Id = 8, ProductId = 8, FileName = "redmi_note12.jpg", FilePath = "/images/products/redmi_note12.jpg" },
                new Image { Id = 9, ProductId = 9, FileName = "pixel7.jpg", FilePath = "/images/products/pixel7.jpg" },
                new Image { Id = 10, ProductId = 10, FileName = "nokia_x20.jpg", FilePath = "/images/products/nokia_x20.jpg" },

                new Image { Id = 11, ProductId = 11, FileName = "mx_master3s.jpg", FilePath = "/images/products/mx_master3s.jpg" },
                new Image { Id = 12, ProductId = 12, FileName = "airpods_pro2.jpg", FilePath = "/images/products/airpods_pro2.jpg" },
                new Image { Id = 13, ProductId = 13, FileName = "anker_20k.jpg", FilePath = "/images/products/anker_20k.jpg" },
                new Image { Id = 14, ProductId = 14, FileName = "sony_ch710n.jpg", FilePath = "/images/products/sony_ch710n.jpg" }
                );
        }
    }
}
