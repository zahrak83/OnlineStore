using Microsoft.AspNetCore.Identity;
using OnlineStore.Domain.Core.Common;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Contract.IService;
using OnlineStore.Domain.Core.Dtos;
using OnlineStore.Domain.Core.Entities;
using OnlineStore.Domain.Core.enums;

namespace OnlineStore.Domain.AppService
{
    public class UserAppService(IUserService userService, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole<int>> roleManager) : IUserAppService
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

        public async Task<Result<UserDto>> LoginAsync(UserLoginDto dto, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByNameAsync(dto.Username);

            if (user == null)
                return Result<UserDto>.Failure("نام کاربری اشتباه است.");

            var signInResult = await signInManager.CheckPasswordSignInAsync(user,dto.Password,lockoutOnFailure: false);

            if (!signInResult.Succeeded)
                return Result<UserDto>.Failure("رمز عبور اشتباه است.");

            await signInManager.SignInAsync(user, isPersistent: false);

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName!,
                Email = user.Email!,
                Balance = user.Balance,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            };

            return Result<UserDto>.Success("ورود با موفقیت انجام شد.", userDto);
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken)
        {
            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Balance = 200000,
                Role = UserRole.Customer
            };

            var result = await userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return Result<UserDto>.Failure( string.Join(" | ", result.Errors.Select(e => e.Description)));

            var roleName = UserRole.Customer.ToString();

            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole<int>(roleName));

            await userManager.AddToRoleAsync(user, roleName);

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName!,
                Email = user.Email!,
                Balance = user.Balance,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            };

            return Result<UserDto>.Success("ثبت‌نام با موفقیت انجام شد.", userDto);
        }

        public async Task<Result<UserDto>> UpdateProfileAsync(UpdateProfileDto dto, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(dto.UserId.ToString());

            if (user == null)
                return Result<UserDto>.Failure("کاربر یافت نشد.");

            if (!string.Equals(user.UserName, dto.Username, StringComparison.OrdinalIgnoreCase))
            {
                var existingUser = await userManager.FindByNameAsync(dto.Username);

                if (existingUser != null && existingUser.Id != user.Id)
                    return Result<UserDto>.Failure("این نام کاربری قبلاً استفاده شده است.");

                user.UserName = dto.Username;
            }

            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Result<UserDto>.Failure(string.Join(" | ", result.Errors.Select(e => e.Description)));

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName!,
                Email = user.Email!,
                Balance = user.Balance,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            };

            return Result<UserDto>.Success("پروفایل با موفقیت ویرایش شد.", userDto);
        }

    }
}
