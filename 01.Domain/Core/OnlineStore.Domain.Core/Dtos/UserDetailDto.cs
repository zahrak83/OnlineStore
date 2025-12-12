namespace OnlineStore.Domain.Core.Dtos
{
    public class UserDetailDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public decimal Balance { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalPrice { get; set; } 
        public List<CartItemDto> CurrentCart { get; set; } = new();
        public List<OrderSummaryDto> Orders { get; set; } = new();
    }
}
