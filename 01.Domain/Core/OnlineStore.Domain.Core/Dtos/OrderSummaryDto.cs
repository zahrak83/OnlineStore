namespace OnlineStore.Domain.Core.Dtos
{
    public class OrderSummaryDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int ItemsCount { get; set; }
    }
}
