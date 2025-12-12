using OnlineStore.Domain.Core.Contract.IRepository;
using OnlineStore.Domain.Core.Contract.IService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Service
{
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {
        public async Task<int> CreateOrderAsync(OrderCreateDto dto, CancellationToken cancellationToken)
        {
            return await orderRepository.CreateOrderAsync(dto, cancellationToken);
        }

        public async Task<List<OrderSummaryForAdminDto>> GetAllOrdersAsync(CancellationToken cancellationToken)
        {
            return await orderRepository.GetAllOrdersAsync(cancellationToken);
        }

        public async Task<OrderDetailDto?> GetOrderDetailAsync(int orderId, CancellationToken cancellationToken)
        {
            return await orderRepository.GetOrderDetailAsync(orderId, cancellationToken);
        }
    }
}
