namespace OnlineStore.Domain.Core.Dtos
{
    public class ImageDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int ProductId { get; set; }
    }
}
