using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStoreWeb.Pages.Admin.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryAppService _categoryService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            ICategoryAppService categoryService,
            ILogger<IndexModel> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        public List<CategoryDto> Categories { get; set; } = new();

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken ct)
        {
            var result = await _categoryService.GetAllAsync(ct);

            if (!result.IsSuccess)
            {
                _logger.LogError("Error retrieving category list: {Message}", result.Message);
                ErrorMessage = "There was an error loading categories.";
                Categories = new();
                return Page();
            }

            Categories = result.Data ?? new();
            _logger.LogInformation("The list of categories was successfully loaded. Number: {Count}", Categories.Count);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                ErrorMessage = "The category ID is invalid.";
                return RedirectToPage();
            }

            var result = await _categoryService.DeleteAsync(id, ct);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to delete category {Id}: {Message}", id, result.Message);
                ErrorMessage = result.Message ?? "خطا در حذف دسته‌بندی.";
            }
            else
            {
                _logger.LogInformation("Category with ID {Id} was successfully deleted..", id);
                SuccessMessage = "Category successfully deleted..";
            }

            return RedirectToPage();
        }
    }
}
