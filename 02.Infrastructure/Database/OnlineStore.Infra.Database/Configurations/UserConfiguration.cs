using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Core.Entities;
using OnlineStore.Domain.Core.enums;

namespace OnlineStore.Infra.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = 1,
                    Username = "ali",
                    Password = "123",
                    Balance = 300000,
                    Role = UserRole.Admin
                },
                new User
                {
                    Id = 2,
                    Username = "zahra.k",
                    Password = "zk123",
                    Balance = 500000,
                    Role = UserRole.Customer
                },
                new User
                {
                    Id = 3,
                    Username = "sahar",
                    Password = "1234",
                    Balance = 400000,
                    Role = UserRole.Customer
                }
            );
        }
    }
}
