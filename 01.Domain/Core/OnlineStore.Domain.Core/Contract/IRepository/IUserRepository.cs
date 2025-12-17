using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Core.Contract.IRepository
{
    public interface IUserRepository
    {
        Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        Task<decimal> GetBalanceAsync(int userId, CancellationToken cancellationToken);
        Task<bool> UpdateBalanceAsync(int userId, decimal newBalance, CancellationToken cancellationToken);
        Task<List<UserDto>> GetAllCustomersAsync(CancellationToken cancellationToken);
        Task<UserDetailDto?> GetCustomerDetailAsync(int userId, CancellationToken cancellationToken);
    }
}
