using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Core.Contract.IRepository;
using OnlineStore.Domain.Core.Dtos;
using OnlineStore.Domain.Core.Entities;
using OnlineStore.Infra.Database;

namespace OnlineStore.Infra.Repository
{
    public class CartItemRepository(AppDbContext context) : ICartItemRepository
    {
        public async Task<List<CartItemDto>> GetUserCartAsync(int userId, CancellationToken cancellationToken)
        {
            return await context.CartItems
                .Where(c => c.UserId == userId)
                .Select(c => new CartItemDto
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    ProductTitle = c.Product.Title,
                    UnitPrice = c.Product.Price
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> AddToCartAsync(int userId, int productId, int quantity, CancellationToken cancellationToken)///***
        {
            var cartItem = await context.CartItems
                .Where(c => c.UserId == userId && c.ProductId == productId)
                .FirstOrDefaultAsync(cancellationToken);

            var stock = await context.Products
                .Where(p => p.Id == productId)
                .Select(p => p.Stock)
                .FirstOrDefaultAsync(cancellationToken);

            if (stock < quantity && cartItem == null ||
                (cartItem != null && stock < cartItem.Quantity + quantity))
            {
                return false;
            }

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                return await context.SaveChangesAsync(cancellationToken) > 0;
            }

            await context.CartItems.AddAsync(new CartItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            }, cancellationToken);

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> UpdateQuantityAsync(int userId, int productId, int quantity, CancellationToken cancellationToken)
        {
            var affected = await context.CartItems
                .Where(c => c.UserId == userId && c.ProductId == productId)
                .ExecuteUpdateAsync(c =>
                    c.SetProperty(x => x.Quantity, quantity),
                    cancellationToken);

            return affected > 0;
        }

        public async Task<bool> RemoveItemAsync(int userId, int productId, CancellationToken cancellationToken)
        {
            var affected = await context.CartItems
                .Where(c => c.UserId == userId && c.ProductId == productId)
                .ExecuteDeleteAsync(cancellationToken);

            return affected > 0;
        }

        public async Task<bool> ClearCartAsync(int userId, CancellationToken cancellationToken)
        {
            var affected = await context.CartItems
                .Where(c => c.UserId == userId)
                .ExecuteDeleteAsync(cancellationToken);

            return affected > 0;
        }
    }
}
