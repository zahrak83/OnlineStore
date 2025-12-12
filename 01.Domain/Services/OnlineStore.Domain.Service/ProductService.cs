using OnlineStore.Domain.Core.Contract.IRepository;
using OnlineStore.Domain.Core.Contract.IService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Service
{
    public class ProductService(IProductRepository repository) : IProductService
    {
        public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await repository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<int> AddProductAsync(ProductDto dto, CancellationToken cancellationToken)
        {
            return await repository.AddAsync(dto, cancellationToken);
        }

        public async Task<bool> UpdateProductAsync(ProductDto dto, CancellationToken cancellationToken)
        {
            return await repository.UpdateProductAsync(dto, cancellationToken);
        }

        public async Task<List<ProductDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await repository.GetAllAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            return await repository.DeleteAsync(id, cancellationToken);
        }

        public async Task<int?> GetStockAsync(int productId, CancellationToken cancellationToken)
        {
            return await repository.GetStockAsync(productId, cancellationToken);
        }

        public Task<List<ProductDto>> FilterAsync(int? categoryId, string? search, string? sort, CancellationToken cancellationToken)
        {
            return repository.FilterAsync(categoryId, search, sort, cancellationToken);
        }
    }
}
