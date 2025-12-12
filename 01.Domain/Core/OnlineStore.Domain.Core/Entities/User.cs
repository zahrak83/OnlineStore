using OnlineStore.Domain.Core.enums;

namespace OnlineStore.Domain.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public UserRole Role { get; set; }

        public List<CartItem> CartItems { get; set; }
        public List<Order> Orders { get; set; }
    }
}
