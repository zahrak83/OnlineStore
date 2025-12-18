using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Core.Contract.IRepository;
using OnlineStore.Domain.Core.Dtos;
using OnlineStore.Domain.Core.Entities;
using OnlineStore.Infra.Database;

namespace OnlineStore.Infra.Repository
{
    public class ProductRepository(AppDbContext context) : IProductRepository
    {
        public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await context.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Price = p.Price,
                    Description = p.Description,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    Images = p.Images.Select(i => i.FilePath).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<ProductDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Products
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Price = p.Price,
                    Description = p.Description,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    Images = p.Images.Select(i => i.FilePath).ToList()
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<int> AddAsync(ProductDto dto, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                Stock = dto.Stock
            };

            await context.Products.AddAsync(product, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }

        public async Task<bool> UpdateProductAsync(ProductDto dto, CancellationToken cancellationToken)
        {
            var affected = await context.Products
                .Where(p => p.Id == dto.Id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(p => p.Title, dto.Title)
                    .SetProperty(p => p.Price, dto.Price)
                    .SetProperty(p => p.Stock, dto.Stock)
                    .SetProperty(p => p.Description, dto.Description)
                    .SetProperty(p => p.CategoryId, dto.CategoryId),
                    cancellationToken);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var affected = await context.Products
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return affected > 0;
        }

        public async Task<List<ProductDto>> FilterAsync(int? categoryId, string? search, string? sort, CancellationToken cancellationToken)
        {
            var query = context.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .AsQueryable();

            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var keyword = search.Trim().ToLower();
                query = query.Where(p => p.Title.ToLower().Contains(keyword));
            }

            query = sort switch
            {
                "title" => query.OrderBy(p => p.Title),
                "price" => query.OrderBy(p => p.Price),
                "stock" => query.OrderBy(p => p.Stock),
                _ => query.OrderBy(p => p.Id)
            };

            return await query.Select(p => new ProductDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                CategoryId = p.CategoryId,
                Images = p.Images.Select(i => i.FilePath).ToList()
            }).ToListAsync(cancellationToken);
        }
        
        public async Task<bool> DecreaseStockAsync(int productId, int quantity, CancellationToken cancellationToken)
        {
            var affected = await context.Products
                .Where(p => p.Id == productId && p.Stock >= quantity)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(x => x.Stock, x => x.Stock - quantity),
                    cancellationToken);

            return affected > 0;
        }

    }
}