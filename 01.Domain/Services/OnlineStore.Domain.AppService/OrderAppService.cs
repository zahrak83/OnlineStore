using OnlineStore.Domain.Core.Common;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Contract.IService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.AppService
{
    public class OrderAppService(IOrderService orderService, IUserService userService, ICartItemService cartItemService, IProductService productService) : IOrderAppService
    {
        public async Task<Result<int>> CreateOrderAsync(OrderCreateDto dto,CancellationToken cancellationToken)
        {
            if (dto.Items == null || dto.Items.Count == 0)
                return Result<int>.Failure("سبد خرید خالی است.");


            foreach (var item in dto.Items)
            {
                var stockDecreased = await productService.DecreaseStockAsync(item.ProductId, item.Quantity, cancellationToken);

                if (!stockDecreased)
                    return Result<int>.Failure($"موجودی محصول با شناسه {item.ProductId} کافی نیست.");
            }

            var balance = await userService.GetBalanceAsync(dto.UserId, cancellationToken);

            if (balance < dto.TotalPrice)
                return Result<int>.Failure("موجودی حساب کافی نیست.");

            var orderId = await orderService.CreateOrderAsync(dto, cancellationToken);

            if (orderId <= 0)
                return Result<int>.Failure("ایجاد سفارش با خطا مواجه شد.");

            var balanceUpdated = await userService.UpdateBalanceAsync(dto.UserId, balance - dto.TotalPrice, cancellationToken);

            if (!balanceUpdated)
                return Result<int>.Failure("خطا در بروزرسانی موجودی حساب.");

            var cartCleared = await cartItemService
                .ClearCartAsync(dto.UserId, cancellationToken);

            if (!cartCleared)
                return Result<int>.Failure("خطا در پاک کردن سبد خرید.");

            return Result<int>.Success("سفارش با موفقیت ثبت شد.", orderId);
        }

        public async Task<Result<List<OrderSummaryForAdminDto>>> GetAllOrdersForAdminAsync(CancellationToken cancellationToken)
        {
            var orders = await orderService.GetAllOrdersAsync(cancellationToken);

            if (orders == null || !orders.Any())
                return Result<List<OrderSummaryForAdminDto>>.Success("هنوز سفارشی ثبت نشده است.", new List<OrderSummaryForAdminDto>());

            return Result<List<OrderSummaryForAdminDto>>.Success("لیست سفارشات با موفقیت دریافت شد.", orders);
        }

        public async Task<Result<OrderDetailDto>> GetOrderDetailForAdminAsync(int orderId, CancellationToken cancellationToken)
        {
            if (orderId <= 0)
                return Result<OrderDetailDto>.Failure("شناسه سفارش نامعتبر است.");

            var detail = await orderService.GetOrderDetailAsync(orderId, cancellationToken);

            if (detail == null)
                return Result<OrderDetailDto>.Failure("سفارش یافت نشد.");

            return Result<OrderDetailDto>.Success("جزئیات سفارش با موفقیت دریافت شد.", detail);
        }
    }
}
