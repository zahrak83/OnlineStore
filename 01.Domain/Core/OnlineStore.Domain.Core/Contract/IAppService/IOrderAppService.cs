using OnlineStore.Domain.Core.Common;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Core.Contract.IAppService
{
    public interface IOrderAppService
    {
        Task<Result<int>> CreateOrderAsync(OrderCreateDto dto, CancellationToken cancellationToken);
        Task<Result<List<OrderSummaryForAdminDto>>> GetAllOrdersForAdminAsync(CancellationToken cancellationToken);
        Task<Result<OrderDetailDto>> GetOrderDetailForAdminAsync(int orderId, CancellationToken cancellationToken);
    }
}
