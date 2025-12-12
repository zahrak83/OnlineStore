using OnlineStore.Domain.Core.Common;
using OnlineStore.Domain.Core.Contract.IAppService;
using OnlineStore.Domain.Core.Contract.IService;
using OnlineStore.Domain.Core.Dtos;


namespace OnlineStore.Domain.AppService
{
    public class ImageAppService(IImageService imageService) : IImageAppService
    {
        public async Task<Result<List<ImageDto>>> UploadProductImagesAsync(UploadImageRequest request, CancellationToken cancellationToken)
        {
            var resultList = new List<ImageDto>();

            if (request.Files == null || request.Files.Count == 0)
                return Result<List<ImageDto>>.Failure("فایلی ارسال نشده است.");

            string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/products");
            Directory.CreateDirectory(uploadFolder);

            foreach (var file in request.Files)
            {
                if (file.Length <= 0)
                    continue;

                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadFolder, fileName);

                await using var stream = File.Create(filePath);
                await file.CopyToAsync(stream, cancellationToken);

                var dto = new ImageDto
                {
                    ProductId = request.ProductId,
                    FileName = fileName,
                    FilePath = "/uploads/products/" + fileName
                };

                await imageService.AddAsync(dto, cancellationToken);
                resultList.Add(dto);
            }

            return Result<List<ImageDto>>.Success("تصاویر با موفقیت ذخیره شدند.", resultList);
        }
    }
}

