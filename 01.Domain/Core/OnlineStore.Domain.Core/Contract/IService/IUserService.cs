using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Core.Contract.IService
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<UserDto?> LoginAsync(UserLoginDto dto, CancellationToken cancellationToken);
        Task<decimal> GetBalanceAsync(int userId, CancellationToken cancellationToken);
        Task<bool> UpdateBalanceAsync(int userId, decimal newBalance, CancellationToken cancellationToken);
        Task<List<UserDto>> GetAllCustomersAsync(CancellationToken cancellationToken);
        Task<UserDetailDto?> GetCustomerDetailAsync(int userId, CancellationToken cancellationToken);
    }
}
