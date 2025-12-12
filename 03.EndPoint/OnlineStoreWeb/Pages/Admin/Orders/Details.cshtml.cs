using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStoreWeb.Pages.Admin.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly IOrderAppService _orderService;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(
            IOrderAppService orderService,
            ILogger<DetailsModel> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        public OrderDetailDto? Order { get; set; }

        [TempData] public string? SuccessMessage { get; set; }
        [TempData] public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, CancellationToken ct)
        {
            if (id <= 0) return NotFound();

            var result = await _orderService.GetOrderDetailForAdminAsync(id, ct);

            if (!result.IsSuccess || result.Data == null)
            {
                ErrorMessage = "سفارش یافت نشد.";
                _logger.LogWarning("ادمین تلاش کرد سفارش {Id} را ببیند — ناموفق", id);
                return RedirectToPage("./Index");
            }

            Order = result.Data;

            _logger.LogInformation("ادمین جزئیات سفارش #{Id} برای مشتری {User} را مشاهده کرد.", id, Order.UserName);

            return Page();
        }
    }

}