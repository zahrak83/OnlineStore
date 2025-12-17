using OnlineStore.Domain.Core.Contract.IRepository;
using OnlineStore.Domain.Core.Contract.IService;
using OnlineStore.Domain.Core.Dtos;

namespace OnlineStore.Domain.Service
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await  userRepository.GetUserByIdAsync(id, cancellationToken);
        }

        public async Task<decimal> GetBalanceAsync(int userId, CancellationToken cancellationToken)
        {
           return await userRepository.GetBalanceAsync(userId, cancellationToken);
        }

        public async Task<bool> UpdateBalanceAsync(int userId, decimal newBalance, CancellationToken cancellationToken)
        {
           return await userRepository.UpdateBalanceAsync(userId, newBalance, cancellationToken);
        }
     
        public async Task<List<UserDto>> GetAllCustomersAsync(CancellationToken cancellationToken)
        {
            return await userRepository.GetAllCustomersAsync(cancellationToken);
        }

        public async Task<UserDetailDto?> GetCustomerDetailAsync(int userId, CancellationToken cancellationToken)
        {
            return await userRepository.GetCustomerDetailAsync(userId, cancellationToken);
        }
    }
}
