using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Core.Contract.IService
{
    public interface IImageService
    {
        Task<List<ImageDto>> GetByProductIdAsync(int productId, CancellationToken cancellationToken);
        Task<bool> AddAsync(ImageDto dto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
