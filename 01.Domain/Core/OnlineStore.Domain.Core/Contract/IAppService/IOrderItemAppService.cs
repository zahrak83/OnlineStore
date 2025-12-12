using OnlineStore.Domain.Core.Common;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Core.Contract.IAppService
{
    public interface IOrderItemAppService
    {
        Task<Result<List<OrderItemDto>>> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken);
    }
}
