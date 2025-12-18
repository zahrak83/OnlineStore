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
        private readonly IUserAppService _userService;
        private readonly UserManager<User> _userManager;

        public UserProfileModel(IUserAppService userService, UserManager<User> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [BindProperty]
        public EditUserProfileDto Input { get; set; } = new();

        [TempData] 
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync() 
        { 
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdStr)) 
                return RedirectToPage("/Login"); 
            
            int userId = int.Parse(userIdStr); 
            
            var user = await _userManager.FindByIdAsync(userId.ToString()); 
            
            if (user == null) { ErrorMessage = "کاربر یافت نشد."; 
                
                return Page(); 
            } 
            
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
            {
                return Page();
            }

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToPage("/Login");

            int userId = int.Parse(userIdStr);

            var updateDto = new UpdateProfileDto
            {
                UserId = userId,
                Username = Input.Username,
                Email = Input.Email,
                PhoneNumber = Input.PhoneNumber
            };

            var updateResult = await _userService.UpdateProfileAsync(updateDto, CancellationToken.None);

            if (!updateResult.IsSuccess)
            {
                ErrorMessage = updateResult.Message;
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
