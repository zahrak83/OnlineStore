using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Core.Contract.IRepository;
using OnlineStore.Domain.Core.Dtos;
using OnlineStore.Domain.Core.Entities;
using OnlineStore.Infra.Database;

namespace OnlineStore.Infra.Repository
{
    public class OrderRepository(AppDbContext context) : IOrderRepository
    {
        public async Task<int> CreateOrderAsync(OrderCreateDto dto, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                UserId = dto.UserId,
                TotalPrice = dto.TotalPrice,
                CreatedAt = DateTime.UtcNow,
                OrderItems = dto.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            await context.Orders.AddAsync(order, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return order.Id; 
        }
        
        public async Task<List<OrderSummaryForAdminDto>> GetAllOrdersAsync(CancellationToken cancellationToken)
        {
            return await context.Orders
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new OrderSummaryForAdminDto
                {
                    Id = o.Id,
                    UserName = o.User.Username,
                    TotalPrice = o.TotalPrice,
                    CreatedAt = o.CreatedAt,
                    ItemsCount = o.OrderItems.Sum(oi => oi.Quantity)
                })
                .ToListAsync(cancellationToken);
        }
        
        public async Task<OrderDetailDto?> GetOrderDetailAsync(int orderId, CancellationToken cancellationToken)
        {
            return await context.Orders
                .Where(o => o.Id == orderId)
                .Select(o => new OrderDetailDto
                {
                    Id = o.Id,
                    UserName = o.User.Username,
                    TotalPrice = o.TotalPrice,
                    CreatedAt = o.CreatedAt,
                    Items = o.OrderItems.Select(oi => new OrderItemDetailDto
                    {
                        ProductTitle = oi.Product.Title,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
