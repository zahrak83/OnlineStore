using Microsoft.AspNetCore.Http;

namespace OnlineStore.Domain.Core.Dtos
{
    public class UploadImageRequest
    {
        public List<IFormFile> Files { get; set; } = new();
        public int ProductId { get; set; }
    }

}
