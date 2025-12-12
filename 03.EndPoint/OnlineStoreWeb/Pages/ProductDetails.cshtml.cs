using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStoreWeb.Pages
{
    public class ProductDetailsModel : PageModel
    {
        private readonly IProductAppService _productAppService;
        private readonly ICartItemAppService _cartItemAppService;
        private readonly ILogger<ProductDetailsModel> _logger;

        public ProductDetailsModel(
            IProductAppService productAppService,
            ICartItemAppService cartItemAppService,
            ILogger<ProductDetailsModel> logger)
        {
            _productAppService = productAppService;
            _cartItemAppService = cartItemAppService;
            _logger = logger;
        }

        public ProductDto Product { get; set; } = null!;
        public string? ErrorMessage { get; set; }

        [BindProperty]
        public int Quantity { get; set; }
        public bool IsInCart { get; set; }


        public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _productAppService.GetByIdAsync(id, cancellationToken);

            if (!result.IsSuccess || result.Data == null)
            {
                ErrorMessage = "محصول یافت نشد.";
                return Page();
            }

            Product = result.Data;

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                var cartResult = await _cartItemAppService.GetUserCartAsync(userId.Value, cancellationToken);
                if (cartResult.IsSuccess && cartResult.Data != null)
                {
                    IsInCart = cartResult.Data.Any(c => c.ProductId == id);
                }
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAddToCartAsync(int id, CancellationToken cancellationToken)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                ErrorMessage = "برای افزودن به سبد خرید ابتدا وارد شوید.";
                return await OnGetAsync(id, cancellationToken);
            }

            if (Quantity <= 0)
                Quantity = 1;

            var result = await _cartItemAppService.AddToCartAsync(userId.Value, id, Quantity, cancellationToken);

            if (!result.IsSuccess)
            {
                ErrorMessage = result.Message;
                return await OnGetAsync(id, cancellationToken);
            }

            TempData["Success"] = "محصول به سبد خرید اضافه شد.";
            return RedirectToPage("/Cart");
        }

        public async Task<IActionResult> OnPostRemoveFromCartAsync(int id, CancellationToken cancellationToken)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Login");

            await _cartItemAppService.RemoveItemAsync(userId.Value, id, cancellationToken);

            return RedirectToPage(new { id });
        }

    }
}