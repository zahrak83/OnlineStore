using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Core.Contract.IRepository
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderAsync(OrderCreateDto dto, CancellationToken cancellationToken);
        Task<List<OrderSummaryForAdminDto>> GetAllOrdersAsync(CancellationToken ctcancellationToken);
        Task<OrderDetailDto?> GetOrderDetailAsync(int orderId, CancellationToken ctcancellationToken);
    }
}
