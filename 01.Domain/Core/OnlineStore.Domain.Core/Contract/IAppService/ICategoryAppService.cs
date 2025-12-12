using OnlineStore.Domain.Core.Common;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Core.Contract.IAppService
{
    public interface ICategoryAppService
    {
        Task<Result<List<CategoryDto>>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result<CategoryDto?>> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Result<int>> CreateAsync(CategoryDto dto, CancellationToken cancellationToken);
        Task<Result<bool>> UpdateAsync(CategoryDto category, CancellationToken cancellationToken);
        Task<Result<bool>> DeleteAsync(int categoryId, CancellationToken cancellationToken);
    }
}
