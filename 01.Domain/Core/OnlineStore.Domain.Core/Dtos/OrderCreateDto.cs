namespace OnlineStore.Domain.Core.Dtos
{
    public class OrderCreateDto
    {
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemCreateDto> Items { get; set; } = new();
    }
}
