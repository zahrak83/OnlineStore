using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Core.Contract.IRepository
{
    public interface IOrderItemRepository
    {
        Task<List<OrderItemDto>> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken);
    }
}

