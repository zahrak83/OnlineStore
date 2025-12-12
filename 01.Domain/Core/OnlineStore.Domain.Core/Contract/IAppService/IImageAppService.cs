using OnlineStore.Domain.Core.Common;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Core.Contract.IAppService
{
    public interface IImageAppService
    {
        Task<Result<List<ImageDto>>> UploadProductImagesAsync(UploadImageRequest request, CancellationToken cancellationToken);
    }
}
