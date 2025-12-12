namespace OnlineStore.Domain.Core.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; } 
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<string> Images { get; set; } = new();
    }
}
