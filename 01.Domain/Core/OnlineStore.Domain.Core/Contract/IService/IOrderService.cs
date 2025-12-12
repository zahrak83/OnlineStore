using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Core.Contract.IService
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(OrderCreateDto dto, CancellationToken cancellationToken);
        Task<List<OrderSummaryForAdminDto>> GetAllOrdersAsync(CancellationToken cancellationToken);
        Task<OrderDetailDto?> GetOrderDetailAsync(int orderId, CancellationToken cancellationToken);
    }
}
