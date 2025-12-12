using OnlineStore.Domain.Core.Common;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Core.Contract.IAppService
{
    public interface IProductAppService
    {
        Task<Result<ProductDto?>> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Result<List<ProductDto>>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result<ProductDto>> AddProductAsync(ProductDto dto, CancellationToken cancellationToken);
        Task<Result<bool>> UpdateAsync(ProductDto dto, CancellationToken cancellationToken);
        Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<Result<List<ProductDto>>> FilterAsync(int? categoryId, string? search, string? sort, CancellationToken cancellationToken);
    }
}
