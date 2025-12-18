using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;
using OnlineStore.Domain.Core.Entities;

namespace OnlineStoreWeb.Pages
{
    public class ProductDetailsModel : PageModel
    {
        private readonly IProductAppService _productAppService;
        private readonly ICartItemAppService _cartItemAppService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ProductDetailsModel> _logger;

        public ProductDetailsModel(
            IProductAppService productAppService,
            ICartItemAppService cartItemAppService,
            UserManager<User> userManager,
            ILogger<ProductDetailsModel> logger)
        {
            _productAppService = productAppService;
            _cartItemAppService = cartItemAppService;
            _userManager = userManager;
            _logger = logger;
        }

        public ProductDto Product { get; private set; } = null!;
        public string? ErrorMessage { get; set; }

        [BindProperty]
        public int Quantity { get; set; } = 1;

        public bool IsInCart { get; private set; }

        private async Task<int?> TryGetCurrentUserIdAsync()
        {
            if (User.Identity?.IsAuthenticated != true)
                return null;

            var user = await _userManager.GetUserAsync(User);
            return user?.Id;
        }

        public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _productAppService.GetByIdAsync(id, cancellationToken);
            if (!result.IsSuccess || result.Data == null)
            {
                ErrorMessage = "محصول یافت نشد.";
                return Page();
            }

            Product = result.Data;

            var userId = await TryGetCurrentUserIdAsync();
            if (userId.HasValue)
            {
                var cartResult = await _cartItemAppService.GetUserCartAsync(userId.Value, cancellationToken);
                if (cartResult.IsSuccess && cartResult.Data != null)
                {
                    IsInCart = cartResult.Data.Any(c => c.ProductId == id);
                }
            }

            return Page();
        }

        [Authorize]
        public async Task<IActionResult> OnPostAddToCartAsync(int id, CancellationToken cancellationToken)
        {
            var userId = await TryGetCurrentUserIdAsync();
            if (!userId.HasValue)
            {
                return Challenge();
            }

            if (Quantity <= 0)
                Quantity = 1;

            var result = await _cartItemAppService.AddToCartAsync(
                userId.Value,
                id,
                Quantity,
                cancellationToken);

            if (!result.IsSuccess)
            {
                ErrorMessage = result.Message;
                return await OnGetAsync(id, cancellationToken);
            }

            return RedirectToPage("/Cart");
        }
    }
}
