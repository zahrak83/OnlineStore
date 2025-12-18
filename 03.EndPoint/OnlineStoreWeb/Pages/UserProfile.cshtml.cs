using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;
using OnlineStore.Domain.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace OnlineStoreWeb.Pages
{
    [Authorize]
    public class UserProfileModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public UserProfileModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public EditUserProfileDto Input { get; set; } = new();

        [TempData] public string? SuccessMessage { get; set; }
        [TempData] public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // گرفتن شناسه کاربر جاری
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToPage("/Login");

            int userId = int.Parse(userIdStr);
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                ErrorMessage = "کاربر یافت نشد.";
                return Page();
            }

            // پر کردن فرم با داده‌های فعلی
            Input = new EditUserProfileDto
            {
                Username = user.UserName ?? "",
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToPage("/Login");

            int userId = int.Parse(userIdStr);
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                ErrorMessage = "کاربر یافت نشد.";
                return Page();
            }

            // تغییر نام کاربری
            if (!string.Equals(user.UserName, Input.Username, StringComparison.OrdinalIgnoreCase))
            {
                var existingUser = await _userManager.FindByNameAsync(Input.Username);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    ModelState.AddModelError("Input.Username", "این نام کاربری قبلا استفاده شده است.");
                    return Page();
                }
                user.UserName = Input.Username;
            }

            // تغییر ایمیل و شماره تلفن
            user.Email = Input.Email;
            user.PhoneNumber = Input.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                ErrorMessage = string.Join(" | ", updateResult.Errors.Select(e => e.Description));
                return Page();
            }

            SuccessMessage = "اطلاعات پروفایل با موفقیت بروزرسانی شد.";
            return RedirectToPage();
        }

        public class EditUserProfileDto
        {
            [Required(ErrorMessage = "نام کاربری الزامی است.")]
            [StringLength(50, ErrorMessage = "نام کاربری نمی‌تواند بیش از 50 کاراکتر باشد.")]
            public string Username { get; set; } = string.Empty;

            [EmailAddress(ErrorMessage = "ایمیل معتبر نیست.")]
            public string? Email { get; set; }

            [Phone(ErrorMessage = "شماره تلفن معتبر نیست.")]
            public string? PhoneNumber { get; set; }
        }
    }

}
