using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStoreWeb.Pages
{
    public class CartModel : PageModel
    {
        private readonly ICartItemAppService _cartService;
        private readonly ILogger<CartModel> _logger;

        public CartModel(ICartItemAppService cartService, ILogger<CartModel> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        public List<CartItemDto> CartItems { get; set; } = new();
        public decimal TotalPrice { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Login");

            var result = await _cartService.GetUserCartAsync(userId.Value, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Cart load failed: {Message}", result.Message);
                return Page();
            }

            CartItems = result.Data!;
            TotalPrice = CartItems.Sum(i => i.UnitPrice * i.Quantity);

            return Page();
        }

        public async Task<IActionResult> OnPostIncreaseAsync(int productId, CancellationToken cancellationToken)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            var result = await _cartService.GetUserCartAsync(userId.Value, cancellationToken);
            CartItems = result.Data ?? new();

            var item = CartItems.FirstOrDefault(i => i.ProductId == productId);
            var newQuantity = (item?.Quantity ?? 0) + 1;

            await _cartService.UpdateQuantityAsync(userId.Value, productId, newQuantity, cancellationToken);

            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostDecreaseAsync(int productId, CancellationToken cancellationToken)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            var result = await _cartService.GetUserCartAsync(userId.Value, cancellationToken);
            CartItems = result.Data ?? new();

            var item = CartItems.FirstOrDefault(i => i.ProductId == productId);
            var newQuantity = Math.Max((item?.Quantity ?? 1) - 1, 1);

            await _cartService.UpdateQuantityAsync(userId.Value, productId, newQuantity, cancellationToken);

            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostRemoveAsync(int productId, CancellationToken cancellationToken)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            await _cartService.RemoveItemAsync(userId.Value, productId, cancellationToken);

            return RedirectToPage();
        }
    }
}

