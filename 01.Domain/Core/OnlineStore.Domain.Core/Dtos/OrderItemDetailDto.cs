namespace OnlineStore.Domain.Core.Dtos
{
    public class OrderItemDetailDto
    {
        public string ProductTitle { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
