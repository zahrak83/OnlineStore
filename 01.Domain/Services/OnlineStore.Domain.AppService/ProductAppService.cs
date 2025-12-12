using Microsoft.Extensions.Logging;
using OnlineStore.Domain.Core.Common;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Contract.IService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.AppService
{
    public class ProductAppService(IProductService productService, IImageService imageService, ILogger<ProductAppService> logger) : IProductAppService
    {
        public async Task<Result<ProductDto?>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var product = await productService.GetByIdAsync(id, cancellationToken);
            return product is null
                ? Result<ProductDto?>.Failure("محصول یافت نشد.")
                : Result<ProductDto?>.Success("محصول با موفقیت دریافت شد.", product);
        }

        public async Task<Result<List<ProductDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var products = await productService.GetAllAsync(cancellationToken);
            return Result<List<ProductDto>>.Success("لیست محصولات با موفقیت دریافت شد.", products);
        }

        public async Task<Result<ProductDto>> AddProductAsync(ProductDto dto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return Result<ProductDto>.Failure("عنوان محصول الزامی است.");

            if (dto.Price <= 0)
                return Result<ProductDto>.Failure("قیمت باید بزرگتر از صفر باشد.");

            if (dto.Stock < 0)
                return Result<ProductDto>.Failure("موجودی نمی‌تواند منفی باشد.");

            if (dto.CategoryId <= 0)
                return Result<ProductDto>.Failure("دسته‌بندی معتبر انتخاب نشده است.");

            var newProductId = await productService.AddProductAsync(dto, cancellationToken);
            if (newProductId <= 0)
                return Result<ProductDto>.Failure("خطا در ثبت محصول.");

            dto.Id = newProductId;

            if (dto.Images is { Count: > 0 })
            {
                foreach (var path in dto.Images.Distinct()) 
                {
                    var imageDto = new ImageDto
                    {
                        ProductId = newProductId,
                        FilePath = path,
                        FileName = Path.GetFileName(path)
                    };

                    var added = await imageService.AddAsync(imageDto, cancellationToken);
                }
            }

            return Result<ProductDto>.Success("محصول با موفقیت اضافه شد.", dto);
        }

        public async Task<Result<bool>> UpdateAsync(ProductDto dto, CancellationToken cancellationToken)
        {
            if (dto.Id <= 0)
                return Result<bool>.Failure("شناسه محصول نامعتبر است.");

            var existing = await productService.GetByIdAsync(dto.Id, cancellationToken);
            if (existing is null)
                return Result<bool>.Failure("محصول یافت نشد.");

            if (string.IsNullOrWhiteSpace(dto.Title))
                return Result<bool>.Failure("عنوان محصول الزامی است.");

            if (dto.Price <= 0)
                return Result<bool>.Failure("قیمت باید بزرگتر از صفر باشد.");

            if (dto.Stock < 0)
                return Result<bool>.Failure("موجودی نمی‌تواند منفی باشد.");

            var updated = await productService.UpdateProductAsync(dto, cancellationToken);
            if (!updated)
                return Result<bool>.Failure("به‌روزرسانی محصول با خطا مواجه شد.");

            if (dto.Images != null && dto.Images.Any())
            {
                var oldImages = await imageService.GetByProductIdAsync(dto.Id, cancellationToken);
                foreach (var img in oldImages)
                {
                    await imageService.DeleteAsync(img.Id, cancellationToken);
                }

                foreach (var path in dto.Images.Distinct())
                {
                    var imageDto = new ImageDto
                    {
                        ProductId = dto.Id,
                        FilePath = path,
                        FileName = Path.GetFileName(path)
                    };
                    await imageService.AddAsync(imageDto, cancellationToken);
                }

                logger?.LogInformation("تصاویر محصول {Id} به‌روزرسانی شد. تعداد جدید: {Count}", dto.Id, dto.Images.Count);
            }

            return Result<bool>.Success("محصول با موفقیت به‌روزرسانی شد.", true);
        }

        public async Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                return Result<bool>.Failure("شناسه محصول نامعتبر است.");

            var deleted = await productService.DeleteAsync(id, cancellationToken);
            return deleted
                ? Result<bool>.Success("محصول و تصاویر آن با موفقیت حذف شد.", true)
                : Result<bool>.Failure("حذف محصول با خطا مواجه شد.");
        }

        public async Task<Result<List<ProductDto>>> FilterAsync(int? categoryId, string? search, string? sort, CancellationToken cancellationToken)
        {
            var items = await productService.FilterAsync(categoryId, search, sort, cancellationToken);

            logger.LogInformation(
                "Product filtered. Category={Category}, Search='{Search}', Sort='{Sort}', Count={Count}",
                categoryId, search, sort, items.Count);

            return Result<List<ProductDto>>.Success("فیلتر با موفقیت انجام شد.", items);
        }
    }
}