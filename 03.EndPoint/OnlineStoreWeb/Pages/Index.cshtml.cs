using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductAppService _productAppService;
        private readonly ICategoryAppService _categoryAppService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            IProductAppService productAppService,
            ICategoryAppService categoryAppService,
            ILogger<IndexModel> logger)
        {
            _productAppService = productAppService;
            _categoryAppService = categoryAppService;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        [StringLength(100, ErrorMessage = "متن جستجو حداکثر {1} کاراکتر می‌تواند باشد.")]
        public string? Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Sort { get; set; }

        public List<ProductDto> Products { get; set; } = new();
        public List<CategoryDto> Categories { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Home page requested. CategoryId={CategoryId}, SearchTerm={Search}, Sort={Sort}",
                CategoryId, Search, Sort);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "ModelState invalid on Home page request. Errors: {Errors}",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                await LoadCategoriesAsync(cancellationToken);
                return Page();
            }

            await LoadCategoriesAsync(cancellationToken);

            try
            {
                var result = await _productAppService.FilterAsync(CategoryId, Search, Sort, cancellationToken);

                if (!result.IsSuccess || result.Data == null)
                {
                    _logger.LogError("Failed to load products: {Message}", result?.Message ?? "null");
                    ErrorMessage = "خطا در دریافت محصولات. لطفاً بعداً تلاش کنید.";
                    Products = new List<ProductDto>();
                }
                else
                {
                    Products = result.Data;

                    foreach (var p in Products)
                    {
                        if (p.Stock <= 5) 
                        {
                            _logger.LogWarning(
                                "Low stock for product {ProductId} ({Title}) — stock={Stock}",
                                p.Id, p.Title, p.Stock);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while loading products.");
                ErrorMessage = "خطای داخلی رخ داده است.";
            }

            return Page();
        }

        private async Task LoadCategoriesAsync(CancellationToken cancellationToken)
        {
            var catResult = await _categoryAppService.GetAllAsync(cancellationToken);
            if (catResult?.IsSuccess == true && catResult.Data != null)
                Categories = catResult.Data;
        }
    }
}
