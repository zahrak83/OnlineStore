namespace OnlineStore.Domain.Core.Dtos
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDetailDto> Items { get; set; }
    }
}
