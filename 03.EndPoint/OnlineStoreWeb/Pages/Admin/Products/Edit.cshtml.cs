using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OnlineStoreWeb.Pages.Admin.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductAppService _productService;
        private readonly ICategoryAppService _categoryService;
        private readonly ILogger<EditModel> _logger;

        public EditModel(
            IProductAppService productService,
            ICategoryAppService categoryService,
            ILogger<EditModel> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger;
        }

        [BindProperty]
        public ProductInputModel Product { get; set; } = new();

        public List<CategoryDto> Categories { get; set; } = new();

        public List<string> CurrentImages { get; set; } = new();

        [TempData] public string? SuccessMessage { get; set; }
        [TempData] public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                return NotFound();

            await LoadCategoriesAsync(ct);

            var result = await _productService.GetByIdAsync(id, ct);
            if (!result.IsSuccess || result.Data == null)
            {
                ErrorMessage = "محصول یافت نشد.";
                return RedirectToPage("./Index");
            }

            var p = result.Data;
            Product = new ProductInputModel
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                Description = p.Description,
                Stock = p.Stock,
                CategoryId = p.CategoryId
            };

            CurrentImages = p.Images ?? new();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            await LoadCategoriesAsync(ct);

            if (!ModelState.IsValid)
            {
                var current = await _productService.GetByIdAsync(Product.Id, ct);
                CurrentImages = current.Data?.Images ?? new();
                return Page();
            }

            var newImagePaths = new List<string>();

            if (Product.ImageFiles != null && Product.ImageFiles.Count > 0)
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/products");
                Directory.CreateDirectory(uploadFolder);

                foreach (var file in Product.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploadFolder, fileName);
                        await using var stream = System.IO.File.Create(filePath);
                        await file.CopyToAsync(stream);
                        newImagePaths.Add("/uploads/products/" + fileName);
                    }
                }
            }

            var allImages = CurrentImages.Concat(newImagePaths).ToList();

            var dto = new ProductDto
            {
                Id = Product.Id,
                Title = Product.Title.Trim(),
                Price = Product.Price,
                Description = Product.Description?.Trim(),
                Stock = Product.Stock,
                CategoryId = Product.CategoryId,
                Images = allImages
            };

            var result = await _productService.UpdateAsync(dto, ct);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to update product {Id}: {Msg}", Product.Id, result.Message);
                ErrorMessage = result.Message ?? "خطا در به‌روزرسانی محصول.";
                CurrentImages = allImages;
                return Page();
            }

            _logger.LogInformation("Product {Id} updated successfully: {Title}", Product.Id, dto.Title);
            SuccessMessage = $"محصول «{dto.Title}» با موفقیت به‌روزرسانی شد.";
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostRemoveImageAsync(int productId, string imagePath, CancellationToken ct)
        {
            var product = await _productService.GetByIdAsync(productId, ct);
            if (product.Data != null)
            {
                product.Data.Images?.Remove(imagePath);
                await _productService.UpdateAsync(product.Data, ct);
            }
            return new JsonResult(new { success = true });
        }

        private async Task LoadCategoriesAsync(CancellationToken ct)
        {
            var result = await _categoryService.GetAllAsync(ct);
            Categories = result.Data ?? new();
        }

        public class ProductInputModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "عنوان محصول الزامی است.")]
            [StringLength(100, ErrorMessage = "عنوان نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.")]
            public string Title { get; set; } = string.Empty;

            [Required(ErrorMessage = "قیمت الزامی است.")]
            [Range(1000, 100_000_000, ErrorMessage = "قیمت باید بین ۱,۰۰۰ تا ۱۰۰ میلیون تومان باشد.")]
            public decimal Price { get; set; }

            [Display(Name = "توضیحات")]
            public string? Description { get; set; }

            [Required(ErrorMessage = "موجودی الزامی است.")]
            [Range(0, 10000, ErrorMessage = "موجودی باید بین ۰ تا ۱۰,۰۰۰ باشد.")]
            public int Stock { get; set; }

            [Required(ErrorMessage = "دسته‌بندی را انتخاب کنید.")]
            public int CategoryId { get; set; }

            [Display(Name = "تصاویر جدید")]
            public List<IFormFile>? ImageFiles { get; set; }
        }
    }
}

