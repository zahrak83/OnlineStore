using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;
using OnlineStore.Domain.Core.Entities;

namespace OnlineStoreWeb.Pages
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        private readonly ICartItemAppService _cartService;
        private readonly IOrderAppService _orderService;
        private readonly IUserAppService _userService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CheckoutModel> _logger;

        public CheckoutModel(
            ICartItemAppService cartService,
            IOrderAppService orderService,
            IUserAppService userService,
            UserManager<User> userManager,
            ILogger<CheckoutModel> logger)
        {
            _cartService = cartService;
            _orderService = orderService;
            _userService = userService;
            _userManager = userManager;
            _logger = logger;
        }

        public List<CartItemDto> CartItems { get; set; } = new();
        public decimal TotalPrice { get; set; }

        [TempData] public string? ErrorMessage { get; set; }
        [TempData] public string? SuccessMessage { get; set; }

        private async Task<int> GetCurrentUserIdAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) throw new Exception("کاربر وارد نشده است.");
            return user.Id;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            int userId = await GetCurrentUserIdAsync();
            var cartResult = await _cartService.GetUserCartAsync(userId, cancellationToken);

            if (!cartResult.IsSuccess || cartResult.Data == null)
            {
                ErrorMessage = "سبد خرید شما خالی است یا بارگذاری امکان‌پذیر نیست.";
                CartItems = new();
                TotalPrice = 0;
                return Page();
            }

            CartItems = cartResult.Data;
            TotalPrice = CartItems.Sum(c => c.UnitPrice * c.Quantity);
            return Page();
        }

        public async Task<IActionResult> OnPostCheckoutAsync(CancellationToken cancellationToken)
        {
            int userId = await GetCurrentUserIdAsync();
            var cartResult = await _cartService.GetUserCartAsync(userId, cancellationToken);

            if (!cartResult.IsSuccess || cartResult.Data == null || !cartResult.Data.Any())
            {
                ErrorMessage = "سبد خرید شما خالی است.";
                return Page();
            }

            CartItems = cartResult.Data;
            TotalPrice = CartItems.Sum(c => c.UnitPrice * c.Quantity);

            var userResult = await _userService.GetUserAsync(userId, cancellationToken);
            if (!userResult.IsSuccess || userResult.Data == null)
            {
                ErrorMessage = "کاربر یافت نشد.";
                return Page();
            }

            if (userResult.Data.Balance < TotalPrice)
            {
                ErrorMessage = "موجودی حساب شما کافی نیست.";
                return Page();
            }

            var orderDto = new OrderCreateDto
            {
                UserId = userId,
                TotalPrice = TotalPrice,
                Items = CartItems.Select(c => new OrderItemCreateDto
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    UnitPrice = c.UnitPrice
                }).ToList()
            };

            var orderResult = await _orderService.CreateOrderAsync(orderDto, cancellationToken);
            if (!orderResult.IsSuccess)
            {
                ErrorMessage = orderResult.Message;
                return Page();
            }

            SuccessMessage = "سفارش شما با موفقیت ثبت شد.";
            return RedirectToPage("/OrderSuccess", new { id = orderResult.Data });
        }
    }
}
