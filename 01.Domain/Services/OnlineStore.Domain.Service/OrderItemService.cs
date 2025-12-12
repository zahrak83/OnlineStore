using OnlineStore.Domain.Core.Contract.IRepository;
using OnlineStore.Domain.Core.Contract.IService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Service
{
    public class OrderItemService(IOrderItemRepository orderItemRepository) : IOrderItemService
    {
        public async Task<List<OrderItemDto>> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken)
        {
            return await orderItemRepository.GetByOrderIdAsync(orderId, cancellationToken);
        }
    }
}
