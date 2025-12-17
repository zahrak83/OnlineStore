using OnlineStore.Domain.Core.Common;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Contract.IService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.AppService
{
    public class UserAppService(IUserService userService) : IUserAppService
    {
        public async Task<Result<UserDto>> GetUserAsync(int id, CancellationToken cancellationToken)
        {
            var user = await userService.GetByIdAsync(id, cancellationToken);

            if (user == null)
                return Result<UserDto>.Failure("کاربر یافت نشد.");

            return Result<UserDto>.Success("کاربر دریافت شد.", user);
        }

        public async Task<Result<bool>> UpdateBalanceAsync(int userId, decimal amount, CancellationToken cancellationToken)
        {
            if (amount < 0)
                return Result<bool>.Failure("موجودی نمی‌تواند منفی باشد.");

            var updated = await userService.UpdateBalanceAsync(userId, amount, cancellationToken);

            if (!updated)
                return Result<bool>.Failure("بروزرسانی موجودی ناموفق بود.");

            return Result<bool>.Success("موجودی با موفقیت بروزرسانی شد.", true);
        }
        
        public async Task<Result<List<UserDto>>> GetAllCustomersAsync(CancellationToken cancellationToken)
        {
            var customers = await userService.GetAllCustomersAsync(cancellationToken);
            return Result<List<UserDto>>.Success("لیست مشتریان با موفقیت دریافت شد.", customers);
        }

        public async Task<Result<UserDetailDto>> GetCustomerDetailAsync(int userId, CancellationToken cancellationToken)
        {
            if (userId <= 0)
                return Result<UserDetailDto>.Failure("شناسه مشتری نامعتبر است.");

            var detail = await userService.GetCustomerDetailAsync(userId, cancellationToken);
            if (detail == null)
                return Result<UserDetailDto>.Failure("مشتری یافت نشد یا دسترسی مجاز نیست.");

            return Result<UserDetailDto>.Success("جزئیات مشتری با موفقیت دریافت شد.", detail);
        }
    }
}
