using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStoreWeb.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly ICartItemAppService _cartService;
        private readonly IOrderAppService _orderService;
        private readonly IUserAppService _userService;
        private readonly ILogger<CheckoutModel> _logger;

        public CheckoutModel(
            ICartItemAppService cartService,
            IOrderAppService orderService,
            IUserAppService userService,
            ILogger<CheckoutModel> logger)
        {
            _cartService = cartService;
            _orderService = orderService;
            _userService = userService;
            _logger = logger;
        }

        public List<CartItemDto> CartItems { get; set; } = new();
        public decimal TotalPrice { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        [TempData]
        public string? SuccessMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                _logger.LogWarning("Checkout attempt without login.");
                return RedirectToPage("/Login");
            }

            var cartResult = await _cartService.GetUserCartAsync(userId.Value, cancellationToken);
            if (!cartResult.IsSuccess)
            {
                _logger.LogWarning("Failed to load cart for user {UserId}: {Message}", userId, cartResult.Message);
                ErrorMessage = cartResult.Message;
                CartItems = new();
                TotalPrice = 0;
                return Page();
            }

            CartItems = cartResult.Data!;
            TotalPrice = CartItems.Sum(c => c.UnitPrice * c.Quantity);
            return Page();
        }

        public async Task<IActionResult> OnPostCheckoutAsync(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid ModelState during checkout.");
                return Page();
            }

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                _logger.LogWarning("Checkout attempt without login.");
                return RedirectToPage("/Login");
            }

            var cartResult = await _cartService.GetUserCartAsync(userId.Value, cancellationToken);
            if (!cartResult.IsSuccess || cartResult.Data == null || cartResult.Data.Count == 0)
            {
                _logger.LogWarning("Checkout failed: empty cart for user {UserId}.", userId);
                ErrorMessage = "سبد خرید شما خالی است.";
                return Page();
            }

            CartItems = cartResult.Data!;
            TotalPrice = CartItems.Sum(c => c.UnitPrice * c.Quantity);

            var userResult = await _userService.GetUserAsync(userId.Value, cancellationToken);
            if (!userResult.IsSuccess || userResult.Data == null)
            {
                _logger.LogError("User not found during checkout. UserId={UserId}", userId);
                ErrorMessage = "کاربر یافت نشد.";
                return Page();
            }

            if (userResult.Data.Balance < TotalPrice)
            {
                _logger.LogWarning("Insufficient balance for user {UserId}. Required={TotalPrice}, Balance={Balance}",
                    userId, TotalPrice, userResult.Data.Balance);
                ErrorMessage = "موجودی حساب شما کافی نیست.";
                return Page();
            }

            var orderDto = new OrderCreateDto
            {
                UserId = userId.Value,
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
                _logger.LogError("Order creation failed for user {UserId}: {Message}", userId, orderResult.Message);
                ErrorMessage = orderResult.Message;
                return Page();
            }

            _logger.LogInformation("Order {OrderId} successfully created for user {UserId}. TotalPrice={TotalPrice}",
                orderResult.Data, userId, TotalPrice);

            SuccessMessage = "سفارش شما با موفقیت ثبت شد.";
            return RedirectToPage("/OrderSuccess", new { id = orderResult.Data });
        }
    }
}
