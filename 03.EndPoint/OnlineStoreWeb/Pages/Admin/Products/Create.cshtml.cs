using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Server.IIS;
using OnlineStore.Domain.AppService;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OnlineStoreWeb.Pages.Admin.Products
{
    public class CreateModel : PageModel
    {
        private readonly IProductAppService _productService;
        private readonly ICategoryAppService _categoryService;
        private readonly IImageAppService _imageService;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(
            IProductAppService productService,
            ICategoryAppService categoryService,
            IImageAppService imageService,
            ILogger<CreateModel> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _imageService = imageService;
            _logger = logger;
        }

        [BindProperty]
        public ProductInputModel Product { get; set; } = new();

        public List<CategoryDto> Categories { get; set; } = new();

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken ct)
        {
            await LoadCategoriesAsync(ct);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            await LoadCategoriesAsync(ct);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Product creation failed: ModelState invalid.");
                return Page();
            }

            var dto = new ProductDto
            {
                Title = Product.Title.Trim(),
                Price = Product.Price,
                Description = Product.Description?.Trim(),
                Stock = Product.Stock,
                CategoryId = Product.CategoryId
            };

            var createResult = await _productService.AddProductAsync(dto, ct);

            if (!createResult.IsSuccess || createResult.Data == null)
            {
                ErrorMessage = createResult.Message ?? "خطا در ثبت محصول.";
                return Page();
            }

            var productId = createResult.Data.Id;

            if (Product.ImageFiles != null && Product.ImageFiles.Count > 0)
            {
                var uploadRequest = new UploadImageRequest
                {
                    ProductId = productId,
                    Files = Product.ImageFiles
                };

                var uploadResult = await _imageService.UploadProductImagesAsync(uploadRequest, ct);

                if (!uploadResult.IsSuccess)
                {
                    ErrorMessage = uploadResult.Message ?? "خطا در آپلود تصاویر.";
                    return Page();
                }
            }

            SuccessMessage = $"محصول «{dto.Title}» با موفقیت ثبت شد.";
            return RedirectToPage("Index");
        }


        private async Task LoadCategoriesAsync(CancellationToken ct)
        {
            var result = await _categoryService.GetAllAsync(ct);
            Categories = result.Data ?? new List<CategoryDto>();
        }
    }
    public class ProductInputModel
    {
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
        [Display(Name = "دسته‌بندی")]
        public int CategoryId { get; set; }

        [Display(Name = "تصاویر محصول")]
        public List<IFormFile>? ImageFiles { get; set; }
    }
}