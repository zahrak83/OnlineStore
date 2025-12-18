using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace OnlineStoreWeb.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public ForgotPasswordModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "ایمیل الزامی است.")]
            [EmailAddress(ErrorMessage = "ایمیل معتبر نیست.")]
            public string Email { get; set; } = string.Empty;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                TempData["SuccessMessage"] = "لینک بازیابی ارسال شد.";
                return RedirectToPage();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);


            var callbackUrl = Url.Page(
                "/ResetPassword",
                pageHandler: null,
                values: new { email = Input.Email, token },
                protocol: Request.Scheme);

            TempData["ResetLink"] = callbackUrl;
            TempData["SuccessMessage"] = "لینک بازیابی رمز آماده شد.";

            return RedirectToPage();
        }
    }
}