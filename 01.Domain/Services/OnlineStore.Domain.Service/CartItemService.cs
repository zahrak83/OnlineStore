using OnlineStore.Domain.Core.Contract.IRepository;
using OnlineStore.Domain.Core.Contract.IService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Service
{
    public class CartItemService(ICartItemRepository cartItemRepository) : ICartItemService
    {
       public async Task<List<CartItemDto>> GetUserCartAsync(int userId, CancellationToken cancellationToken)
        {
            return await cartItemRepository.GetUserCartAsync(userId, cancellationToken);
        }

        public async Task<bool> AddToCartAsync(int userId, int productId, int quantity, CancellationToken cancellationToken)
        {
            return await cartItemRepository.AddToCartAsync(userId, productId, quantity, cancellationToken);
        }

        public async Task<bool> UpdateQuantityAsync(int userId, int productId, int quantity, CancellationToken cancellationToken)
        {
            return await cartItemRepository.UpdateQuantityAsync(userId, productId, quantity, cancellationToken);
        }

        public async Task<bool> RemoveItemAsync(int userId, int productId, CancellationToken cancellationToken)
        {
            return await cartItemRepository.RemoveItemAsync(userId, productId, cancellationToken);
        }

        public async Task<bool> ClearCartAsync(int userId, CancellationToken cancellationToken)
        {
            return await cartItemRepository.ClearCartAsync(userId, cancellationToken);
        }
    }
}
