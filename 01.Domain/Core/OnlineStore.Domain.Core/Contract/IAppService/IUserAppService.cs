using OnlineStore.Domain.Core.Common;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Core.Contract.IAppService
{
    public interface IUserAppService
    {
        Task<Result<UserDto>> LoginAsync(UserLoginDto dto, CancellationToken cancellationToken);
        Task<Result<UserDto>> GetUserAsync(int id, CancellationToken cancellationToken);
        Task<Result<bool>> UpdateBalanceAsync(int userId, decimal amount, CancellationToken cancellationToken);
        Task<Result<List<UserDto>>> GetAllCustomersAsync(CancellationToken cancellationToken);
        Task<Result<UserDetailDto>> GetCustomerDetailAsync(int userId, CancellationToken cancellationToken);
    }
}
