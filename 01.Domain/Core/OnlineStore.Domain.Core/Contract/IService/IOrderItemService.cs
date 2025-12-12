using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Core.Contract.IService
{
    public interface IOrderItemService
    {
        Task<List<OrderItemDto>> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken);
    }
}
