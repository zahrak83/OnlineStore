using OnlineStore.Domain.Core.Common;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Core.Contract.IAppService
{
    public interface ICartItemAppService
    {
        Task<Result<List<CartItemDto>>> GetUserCartAsync(int userId, CancellationToken cancellationToken);
        Task<Result<bool>> AddToCartAsync(int userId, int productId, int quantity, CancellationToken cancellationToken);
        Task<Result<bool>> UpdateQuantityAsync(int userId, int productId, int quantity, CancellationToken cancellationToken);
        Task<Result<bool>> RemoveItemAsync(int userId, int productId, CancellationToken cancellationToken);
    }
}
