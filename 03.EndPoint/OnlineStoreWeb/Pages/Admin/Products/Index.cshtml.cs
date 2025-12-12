using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStoreWeb.Pages.Admin.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductAppService _productService;
        private readonly ICategoryAppService _categoryService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IProductAppService productService, ICategoryAppService categoryService, ILogger<IndexModel> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger;
        }

        public List<ProductDto> Products { get; set; } = new();
        [TempData] public string? SuccessMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken ct)
        {
            var result = await _productService.GetAllAsync(ct);
            if (result.IsSuccess)
            {
                Products = result.Data ?? new();
                var cats = await _categoryService.GetAllAsync(ct);
                var catDict = cats.Data?.ToDictionary(c => c.Id, c => c.Name) ?? new();
                foreach (var p in Products)
                    p.CategoryName = catDict.GetValueOrDefault(p.CategoryId, "نامشخص");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken ct)
        {
            var result = await _productService.DeleteAsync(id, ct);
            if (result.IsSuccess)
            {
                _logger.LogInformation("Product {Id} deleted by admin.", id);
                SuccessMessage = "محصول با موفقیت حذف شد.";
            }
            else
            {
                _logger.LogWarning("Failed to delete product {Id}: {Msg}", id, result.Message);
            }
            return RedirectToPage();
        }
    }
}
