using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Domain.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
