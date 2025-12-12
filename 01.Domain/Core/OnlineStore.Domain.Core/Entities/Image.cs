namespace OnlineStore.Domain.Core.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
