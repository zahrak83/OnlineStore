using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OnlineStoreWeb.Pages.Admin.Categories
{
    public class EditModel : PageModel
    {
        private readonly ICategoryAppService _categoryService;
        private readonly ILogger<EditModel> _logger;

        public EditModel(
            ICategoryAppService categoryService,
            ILogger<EditModel> logger)
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

        public async Task<IActionResult> OnGetAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                return NotFound();

            var result = await _categoryService.GetByIdAsync(id, ct);

            if (!result.IsSuccess || result.Data == null)
            {
                ErrorMessage = "دسته‌بندی یافت نشد.";
                return RedirectToPage("./Index");
            }

            var cat = result.Data;
            Category = new CategoryInputModel
            {
                Id = cat.Id,
                Name = cat.Name,
                Description = cat.Description
            };

            _logger.LogInformation("The {Id} - {Name} category edit page has opened.", id, cat.Name);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Category edit failed — validation error.");
                return Page();
            }

            var dto = new CategoryDto
            {
                Id = Category.Id,
                Name = Category.Name.Trim(),
                Description = Category.Description?.Trim()
            };

            var result = await _categoryService.UpdateAsync(dto, ct);

            if (!result.IsSuccess)
            {
                _logger.LogError("Error editing category {Id}: {Message}", Category.Id, result.Message);
                ErrorMessage = result.Message ?? "خطایی در به‌روزرسانی رخ داد.";
                return Page();
            }

            _logger.LogInformation("Category {Id} successfully edited: {NewName}", Category.Id, dto.Name);
            SuccessMessage = $"دسته‌بندی «{dto.Name}» با موفقیت به‌روزرسانی شد.";
            return RedirectToPage("./Index");
        }

        public class CategoryInputModel
        {
            public int Id { get; set; }

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
