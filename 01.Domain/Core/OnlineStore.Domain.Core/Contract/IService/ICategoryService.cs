using OnlineStore.Domain.Core.Dtos;
using OnlineStore.Domain.Core.Entities;

namespace OnlineStore.Domain.Core.Contract.IService
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> CreateAsync(CategoryDto dto, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(CategoryDto category, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int categoryId, CancellationToken cancellationToken);
    }
}
