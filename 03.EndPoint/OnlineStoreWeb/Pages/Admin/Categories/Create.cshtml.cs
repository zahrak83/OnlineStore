using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OnlineStoreWeb.Pages.Admin.Categories
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ICategoryAppService _categoryService;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(
            ICategoryAppService categoryService,
            ILogger<CreateModel> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [BindProperty]
        public CategoryInputModel Category { get; set; } = new();

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Category not created — Form validation error.");
                return Page();
            }

            var dto = new CategoryDto
            {
                Name = Category.Name.Trim(),
                Description = Category.Description?.Trim()
            };

            var result = await _categoryService.CreateAsync(dto, ct);

            if (!result.IsSuccess)
            {
                _logger.LogError("Error creating category: {Message}", result.Message);
                ErrorMessage = result.Message ?? "خطایی در ثبت دسته‌بندی رخ داد.";
                return Page();
            }

            _logger.LogInformation("New category created.: {Name} (ID: {Id})", dto.Name, result.Data);

            SuccessMessage = $"Category <<{dto.Name}>> was successfully created.";
            return RedirectToPage("./Index");
        }

        public class CategoryInputModel
        {
            [Required(ErrorMessage = "نام دسته‌بندی الزامی است.")]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "نام باید بین ۲ تا ۵۰ کاراکتر باشد.")]
            [Display(Name = "نام دسته‌بندی")]
            public string Name { get; set; } = string.Empty;

            [StringLength(500, ErrorMessage = "توضیحات نمی‌تواند بیشتر از ۵۰۰ کاراکتر باشد.")]
            [Display(Name = "توضیحات (اختیاری)")]
            public string? Description { get; set; }
        }
    }
}

