using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Core.Contract.IService
{
    public interface IProductService
    {
        Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> AddProductAsync(ProductDto dto, CancellationToken cancellationToken);
        Task<bool> UpdateProductAsync(ProductDto dto, CancellationToken cancellationToken);
        Task<List<ProductDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<int?> GetStockAsync(int productId, CancellationToken cancellationToken);
        Task<List<ProductDto>> FilterAsync(int? categoryId, string? search, string? sort, CancellationToken cancellationToken);
    }
}
