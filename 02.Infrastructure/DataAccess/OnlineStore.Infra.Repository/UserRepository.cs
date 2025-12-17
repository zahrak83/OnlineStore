using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Core.Contract.IRepository;
using OnlineStore.Domain.Core.Dtos;
using OnlineStore.Domain.Core.Entities;
using OnlineStore.Domain.Core.enums;
using OnlineStore.Infra.Database;

namespace OnlineStore.Infra.Repository
{
    public class UserRepository(AppDbContext context, UserManager<User> userManager) : IUserRepository
    {
        public async Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Balance = u.Balance,
                    Role = u.Role
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<decimal> GetBalanceAsync(int userId, CancellationToken cancellationToken)
        {
            return await context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Balance)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> UpdateBalanceAsync(int userId, decimal newBalance, CancellationToken cancellationToken)
        {
            var affected = await context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(u =>
                u.SetProperty(x => x.Balance, newBalance),
                cancellationToken);

            return affected > 0;
        }

        public async Task<List<UserDto>> GetAllCustomersAsync(CancellationToken cancellationToken)
        {
            return await context.Users
                .Where(u => u.Role == UserRole.Customer) 
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Balance = u.Balance,
                    Role = u.Role
                })
                .OrderBy(u => u.Username)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<UserDetailDto?> GetCustomerDetailAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .Where(u => u.Id == userId && u.Role == UserRole.Customer)
                .Select(u => new UserDetailDto
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Balance = u.Balance,
                    TotalOrders = u.Orders.Count,
                    TotalPrice = u.Orders.Sum(o => o.TotalPrice),
                    CurrentCart = u.CartItems.Select(c => new CartItemDto
                    {
                        ProductId = c.ProductId,
                        Quantity = c.Quantity,
                        ProductTitle = c.Product.Title,
                        UnitPrice = c.Product.Price
                    }).ToList(),
                    Orders = u.Orders.Select(o => new OrderSummaryDto
                    {
                        OrderId = o.Id,
                        OrderDate = o.CreatedAt,
                        TotalPrice = o.TotalPrice,
                        ItemsCount = o.OrderItems.Sum(oi => oi.Quantity)
                    })
                    .OrderByDescending(o => o.OrderDate)
                    .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return user;
        }
    }
}
