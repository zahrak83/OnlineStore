using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Core.Contract.IRepository;
using OnlineStore.Domain.Core.Dtos;
using OnlineStore.Domain.Core.Entities;
using OnlineStore.Infra.Database;

namespace OnlineStore.Infra.Repository
{
    public class ImageRepository(AppDbContext context) : IImageRepository
    {
        public async Task<List<ImageDto>> GetByProductIdAsync(int productId, CancellationToken cancellationToken)
        {
            return await context.Images
                .Where(i => i.ProductId == productId)
                .Select(i => new ImageDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    FileName = i.FileName,
                    FilePath = i.FilePath
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> AddAsync(ImageDto dto, CancellationToken cancellationToken)
        {
            var entity = new Image
            {
                ProductId = dto.ProductId,
                FileName = dto.FileName,
                FilePath = dto.FilePath
            };

            await context.Images.AddAsync(entity, cancellationToken);
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var affected = await context.Images
                .Where(i => i.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            return affected > 0;
        }
    }
}
