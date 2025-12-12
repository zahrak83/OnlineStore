using OnlineStore.Domain.Core.Common;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Contract.IService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.AppService
{
    public class CategoryAppService(ICategoryService categoryService) : ICategoryAppService
    {
        public async Task<Result<List<CategoryDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var categories = await categoryService.GetAllAsync(cancellationToken);
            if (categories == null)
                return Result<List<CategoryDto>>.Failure("دسته‌بندی‌ای یافت نشد.");

            return Result<List<CategoryDto>>.Success("دسته‌بندی‌ها با موفقیت دریافت شد.", categories);
        }

        public async Task<Result<CategoryDto?>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var category = await categoryService.GetByIdAsync(id, cancellationToken);
            if (category == null)
                return Result<CategoryDto?>.Failure("دسته‌بندی یافت نشد.");

            return Result<CategoryDto?>.Success("دسته‌بندی با موفقیت دریافت شد.", category);
        }

        public async Task<Result<int>> CreateAsync(CategoryDto dto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return Result<int>.Failure("نام دسته‌بندی الزامی است.");

            if (dto.Description.Length > 500)
                return Result<int>.Failure("توضیحات دسته‌بندی نمی‌تواند بیشتر از 500 کاراکتر باشد.");

            var newId = await categoryService.CreateAsync(dto, cancellationToken);

            if (newId <= 0)
                return Result<int>.Failure("ایجاد دسته‌بندی با خطا مواجه شد.");

            return Result<int>.Success("دسته‌بندی با موفقیت ایجاد شد.", newId);
        }

        public async Task<Result<bool>> UpdateAsync(CategoryDto category, CancellationToken cancellationToken)
        {
            if (category == null)
                return Result<bool>.Failure("دسته‌بندی نمی‌تواند خالی باشد.");

            if (category.Id <= 0)
                return Result<bool>.Failure("شناسه دسته‌بندی نامعتبر است.");

            if (string.IsNullOrWhiteSpace(category.Name))
                return Result<bool>.Failure("نام دسته‌بندی نمی‌تواند خالی باشد.");

            if (!string.IsNullOrEmpty(category.Description) && category.Description.Length > 500)
                return Result<bool>.Failure("توضیحات دسته‌بندی نباید بیشتر از 500 کاراکتر باشد.");

            var result = await categoryService.UpdateAsync(category, cancellationToken);
            return result
                ? Result<bool>.Success("دسته‌بندی با موفقیت به‌روزرسانی شد.", true)
                : Result<bool>.Failure("به‌روزرسانی دسته‌بندی موفقیت‌آمیز نبود.");
        }

        public async Task<Result<bool>> DeleteAsync(int categoryId, CancellationToken cancellationToken)
        {
            if (categoryId <= 0)
                return Result<bool>.Failure("شناسه دسته‌بندی نامعتبر است.");

            var result = await categoryService.DeleteAsync(categoryId, cancellationToken);
            return result
                ? Result<bool>.Success("دسته‌بندی با موفقیت حذف شد.", true)
                : Result<bool>.Failure("حذف دسته‌بندی موفقیت‌آمیز نبود.");
        }
    }
}
