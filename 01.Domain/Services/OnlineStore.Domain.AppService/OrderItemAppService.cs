using OnlineStore.Domain.Core.Common;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Contract.IService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.AppService
{
    public class OrderItemAppService(IOrderItemService orderItemService) : IOrderItemAppService
    {
        public async Task<Result<List<OrderItemDto>>> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken)
        {
            var items = await orderItemService.GetByOrderIdAsync(orderId, cancellationToken);

            if (items == null || items.Count == 0)
                return Result<List<OrderItemDto>>.Failure("هیچ آیتمی برای این سفارش یافت نشد.");

            return Result<List<OrderItemDto>>.Success("آیتم‌های سفارش با موفقیت دریافت شدند.", items);
        }
    }
}
