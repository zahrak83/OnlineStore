using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStoreWeb.Pages.Admin.Customers
{
    public class IndexModel : PageModel
    {
        private readonly IUserAppService _userService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            IUserAppService userService,
            ILogger<IndexModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public List<UserDto> Customers { get; set; } = new();

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken ct)
        {
            var role = HttpContext.Session.GetInt32("UserRole");
            if (role != 1) 
            {
                _logger.LogWarning("دسترسی غیرمجاز به پنل مشتریان توسط UserId={UserId}", HttpContext.Session.GetInt32("UserId"));
                return RedirectToPage("/AccessDenied");
            }

            var result = await _userService.GetAllCustomersAsync(ct);

            if (!result.IsSuccess)
            {
                _logger.LogError("خطا در دریافت لیست مشتریان: {Message}", result.Message);
                ErrorMessage = "خطایی در بارگذاری مشتریان رخ داد.";
                Customers = new();
                return Page();
            }

            Customers = result.Data ?? new List<UserDto>();

            _logger.LogInformation("لیست مشتریان با موفقیت بارگذاری شد. تعداد: {Count}", Customers.Count);

            return Page();
        }
    }
}
