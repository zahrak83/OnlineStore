using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Core.Contract.IService
{
    public interface ICartItemService
    {
        Task<List<CartItemDto>> GetUserCartAsync(int userId, CancellationToken cancellationToken);
        Task<bool> AddToCartAsync(int userId, int productId, int quantity, CancellationToken cancellationToken);
        Task<bool> UpdateQuantityAsync(int userId, int productId, int quantity, CancellationToken cancellationToken);
        Task<bool> RemoveItemAsync(int userId, int productId, CancellationToken cancellationToken);
        Task<bool> ClearCartAsync(int userId, CancellationToken cancellationToken);
    }
}
