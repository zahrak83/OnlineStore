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
    public class CartModel : PageModel
    {
        private readonly ICartItemAppService _cartService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CartModel> _logger;

        public CartModel(
            ICartItemAppService cartService,
            UserManager<User> userManager,
            ILogger<CartModel> logger)
        {
            _cartService = cartService;
            _userManager = userManager;
            _logger = logger;
        }

        public List<CartItemDto> CartItems { get; set; } = new();
        public decimal TotalPrice { get; set; }

        private async Task<int> GetCurrentUserIdAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("کاربر وارد نشده است.");
                throw new Exception("کاربر وارد نشده است");
            }
            return user.Id;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            int userId = await GetCurrentUserIdAsync();

            var result = await _cartService.GetUserCartAsync(userId, cancellationToken);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("بارگذاری سبد خرید شکست خورد: {Message}", result.Message);
                return Page();
            }

            CartItems = result.Data ?? new();
            TotalPrice = CartItems.Sum(i => i.UnitPrice * i.Quantity);

            return Page();
        }

        public async Task<IActionResult> OnPostIncreaseAsync(int productId, CancellationToken cancellationToken)
        {
            int userId = await GetCurrentUserIdAsync();

            var result = await _cartService.GetUserCartAsync(userId, cancellationToken);
            CartItems = result.Data ?? new();

            var item = CartItems.FirstOrDefault(i => i.ProductId == productId);
            var newQuantity = (item?.Quantity ?? 0) + 1;

            await _cartService.UpdateQuantityAsync(userId, productId, newQuantity, cancellationToken);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDecreaseAsync(int productId, CancellationToken cancellationToken)
        {
            int userId = await GetCurrentUserIdAsync();

            var result = await _cartService.GetUserCartAsync(userId, cancellationToken);
            CartItems = result.Data ?? new();

            var item = CartItems.FirstOrDefault(i => i.ProductId == productId);
            var newQuantity = Math.Max((item?.Quantity ?? 1) - 1, 1);

            await _cartService.UpdateQuantityAsync(userId, productId, newQuantity, cancellationToken);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int productId, CancellationToken cancellationToken)
        {
            int userId = await GetCurrentUserIdAsync();

            await _cartService.RemoveItemAsync(userId, productId, cancellationToken);

            return RedirectToPage();
        }
    }
}
