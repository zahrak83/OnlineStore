using Microsoft.AspNetCore.Identity;
using OnlineStore.Domain.Core.enums;

namespace OnlineStore.Domain.Core.Entities
{
    public class User:IdentityUser<int>
    {
        public decimal Balance { get; set; }
        public UserRole Role { get; set; }

        public List<CartItem> CartItems { get; set; }
        public List<Order> Orders { get; set; }
    }
}
