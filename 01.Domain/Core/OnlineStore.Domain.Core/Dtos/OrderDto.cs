namespace OnlineStore.Domain.Core.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
