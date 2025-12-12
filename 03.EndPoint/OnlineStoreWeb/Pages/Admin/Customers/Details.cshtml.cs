using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStoreWeb.Pages.Admin.Customers
{
    public class DetailsModel : PageModel
    {
        private readonly IUserAppService _userService;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(
            IUserAppService userService,
            ILogger<DetailsModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public UserDetailDto? Customer { get; set; }

        [TempData] public string? SuccessMessage { get; set; }
        [TempData] public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, CancellationToken ct)
        {
            if (id <= 0) return NotFound();

            var result = await _userService.GetCustomerDetailAsync(id, ct);

            if (!result.IsSuccess || result.Data == null)
            {
                ErrorMessage = "مشتری یافت نشد یا دسترسی مجاز نیست.";
                _logger.LogWarning("ادمین تلاش کرد مشتری {Id} را ببیند — ناموفق", id);
                return RedirectToPage("./Index");
            }

            Customer = result.Data;
            _logger.LogInformation("ادمین جزئیات مشتری {Id} - @{Username} را مشاهده کرد.", id, Customer.Username);

            return Page();
        }
    }
}