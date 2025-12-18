using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OnlineStoreWeb.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IUserAppService _userAppService;

        public RegisterModel(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [BindProperty]
        [Required(ErrorMessage = "نام کاربری الزامی است.")]
        public string Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "رمز عبور الزامی است.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [BindProperty]
        [Compare(nameof(Password), ErrorMessage = "رمز عبور و تکرار آن یکسان نیست.")]
        public string ConfirmPassword { get; set; }

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _userAppService.RegisterAsync(new RegisterDto
            {
                UserName = Username,
                Password = Password
            }, HttpContext.RequestAborted);

            if (!result.IsSuccess)
            {
                ErrorMessage = result.Message;
                return Page();
            }

            return RedirectToPage("/Login");
        }
    }
}
