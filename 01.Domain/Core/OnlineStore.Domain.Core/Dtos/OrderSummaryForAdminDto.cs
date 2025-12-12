namespace OnlineStore.Domain.Core.Dtos
{
    public class OrderSummaryForAdminDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ItemsCount { get; set; }
    }
}
