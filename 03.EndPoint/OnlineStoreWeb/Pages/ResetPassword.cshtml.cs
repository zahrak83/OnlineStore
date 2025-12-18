using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace OnlineStoreWeb.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public ResetPasswordModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Email { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Token { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "رمز جدید الزامی است.")]
            [DataType(DataType.Password)]
            [MinLength(6, ErrorMessage = "رمز عبور باید حداقل ۶ کاراکتر باشد.")]
            public string NewPassword { get; set; } = string.Empty;

            [Required(ErrorMessage = "تایید رمز الزامی است.")]
            [DataType(DataType.Password)]
            [Compare("NewPassword", ErrorMessage = "رمزها مطابقت ندارند.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Token))
                return RedirectToPage("/Login");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByEmailAsync(Email!);
            if (user == null)
            {
                TempData["SuccessMessage"] = "رمز عبور با موفقیت تغییر یافت.";
                return RedirectToPage("/Login");
            }

            var result = await _userManager.ResetPasswordAsync(user, Token!, Input.NewPassword);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "رمز عبور با موفقیت تغییر یافت. حالا می‌توانید وارد شوید.";
                return RedirectToPage("/Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}