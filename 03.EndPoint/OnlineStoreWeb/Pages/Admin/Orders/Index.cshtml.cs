using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStoreWeb.Pages.Admin.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IOrderAppService _orderService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            IOrderAppService orderService,
            ILogger<IndexModel> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        public List<OrderSummaryForAdminDto> Orders { get; set; } = new();

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken ct)
        {
            var result = await _orderService.GetAllOrdersForAdminAsync(ct);

            if (!result.IsSuccess)
            {
                _logger.LogError("خطا در دریافت لیست سفارشات ادمین: {Message}", result.Message);
                ErrorMessage = "خطایی در بارگذاری سفارشات رخ داد.";
                Orders = new();
                return Page();
            }

            Orders = result.Data ?? new();

            _logger.LogInformation("ادمین لیست سفارشات را مشاهده کرد. تعداد: {Count}", Orders.Count);

            return Page();
        }
    }
}