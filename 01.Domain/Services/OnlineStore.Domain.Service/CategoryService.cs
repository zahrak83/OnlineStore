using OnlineStore.Domain.Core.Contract.IRepository;
using OnlineStore.Domain.Core.Contract.IService;
using OnlineStore.Domain.Core.Dtos;
using OnlineStore.Domain.Core.Entities;
using OnlineStore.Infra.Repository;

namespace OnlineStore.Domain.Service
{
    public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
    {
        public async Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await categoryRepository.GetAllAsync(cancellationToken);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await categoryRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<int> CreateAsync(CategoryDto dto, CancellationToken cancellationToken)
        {
            return await categoryRepository.CreateAsync(dto, cancellationToken);
        }

        public async Task<bool> UpdateAsync(CategoryDto category, CancellationToken cancellationToken)
        {
            return await categoryRepository.UpdateAsync(category, cancellationToken);
        }

        public async Task<bool> DeleteAsync(int categoryId, CancellationToken cancellationToken)
        {
            return await categoryRepository.DeleteAsync(categoryId, cancellationToken);
        }
    }
}
