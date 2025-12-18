using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Core.Contract.IRepository
{
    public interface IProductRepository
    {
        Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<ProductDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<int> AddAsync(ProductDto productDto, CancellationToken cancellationToken);
        Task<bool> UpdateProductAsync(ProductDto dto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<int?> GetStockAsync(int productId, CancellationToken cancellationToken);
        Task<List<ProductDto>> FilterAsync(int? categoryId, string? search, string? sort, CancellationToken cancellationToken);
        Task<bool> DecreaseStockAsync(int productId, int quantity, CancellationToken cancellationToken);
    }
}
