using OnlineStore.Domain.Core.Common;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Contract.IService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.AppService
{
    public class CartItemAppService(ICartItemService cartItemService) : ICartItemAppService
    {
        public async Task<Result<List<CartItemDto>>> GetUserCartAsync(int userId, CancellationToken cancellationToken)
        {
            var items = await cartItemService.GetUserCartAsync(userId, cancellationToken);

            if (items == null || items.Count == 0)
                return Result<List<CartItemDto>>.Success("سبد خرید خالی است.", new List<CartItemDto>());

            var totalPrice = items.Sum(i => i.UnitPrice * i.Quantity);

            var message = $"سبد خرید با موفقیت دریافت شد. جمع کل: {totalPrice:N0} $";

            return Result<List<CartItemDto>>.Success(message, items);
        }

        public async Task<Result<bool>> AddToCartAsync(int userId, int productId, int quantity, CancellationToken cancellationToken)
        {
            if (userId <= 0)
                return Result<bool>.Failure("کاربر شناسایی نشد.");

            if (productId <= 0) 
                return Result<bool>.Failure("محصول نامعتبر است.");

            if (quantity <= 0)
                return Result<bool>.Failure("تعداد باید بیشتر از صفر باشد.");

            var added = await cartItemService.AddToCartAsync(userId, productId, quantity, cancellationToken);

            if (!added)
            {
                return Result<bool>.Failure("موجودی محصول کافی نیست.");
            }

            return Result<bool>.Success("محصول با موفقیت به سبد خرید اضافه شد.", true);
        }

        public async Task<Result<bool>> UpdateQuantityAsync(int userId, int productId, int quantity, CancellationToken cancellationToken)
        {
            var updated = await cartItemService.UpdateQuantityAsync(userId, productId, quantity, cancellationToken);
            if (!updated)
                return Result<bool>.Failure("به‌روزرسانی تعداد محصول با خطا مواجه شد.");

            return Result<bool>.Success("تعداد محصول با موفقیت به‌روزرسانی شد.", true);
        }

        public async Task<Result<bool>> RemoveItemAsync(int userId, int productId, CancellationToken cancellationToken)
        {
            var removed = await cartItemService.RemoveItemAsync(userId, productId, cancellationToken);
            if (!removed)
                return Result<bool>.Failure("حذف محصول از سبد خرید با خطا مواجه شد.");

            return Result<bool>.Success("محصول با موفقیت از سبد خرید حذف شد.", true);
        }
    }
}

