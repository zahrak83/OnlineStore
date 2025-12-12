using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Core.Contract.IRepository;
using OnlineStore.Domain.Core.Dtos;
using OnlineStore.Domain.Core.Entities;
using OnlineStore.Infra.Database;

namespace OnlineStore.Infra.Repository
{
    public class CategoryRepository(AppDbContext context) : ICategoryRepository
    {
        public async Task<int> CreateAsync(CategoryDto dto, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await context.Categories.AddAsync(category, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return category.Id; 
        }

        public async Task<bool> DeleteAsync(int categoryId, CancellationToken cancellationToken)
        {
            var affected = await context.Categories
                .Where(c => c.Id == categoryId)
                .ExecuteDeleteAsync(cancellationToken);

            return affected > 0;
        }

        public async Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await context.Categories
                .Where(c => c.Id == id)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> UpdateAsync(CategoryDto dto, CancellationToken cancellationToken)
        {
            var affected = await context.Categories
                .Where(c => c.Id == dto.Id)
                .ExecuteUpdateAsync(c => c
                    .SetProperty(x => x.Name, dto.Name)
                    .SetProperty(x => x.Description, dto.Description),
                    cancellationToken);

            return affected > 0;
        }

    }
}
