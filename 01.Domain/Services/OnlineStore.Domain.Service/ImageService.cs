using OnlineStore.Domain.Core.Contract.IRepository;
using OnlineStore.Domain.Core.Contract.IService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Service
{
    public class ImageService(IImageRepository repository) : IImageService
    {

        public async Task<List<ImageDto>> GetByProductIdAsync(int productId, CancellationToken cancellationToken)
        {
            return await repository.GetByProductIdAsync(productId, cancellationToken);
        }

        public async Task<bool> AddAsync(ImageDto dto, CancellationToken cancellationToken)
        {
            return await repository.AddAsync(dto, cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            return await repository.DeleteAsync(id, cancellationToken);
        }
    }
}
