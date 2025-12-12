using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Core.Contract.IRepository;
using OnlineStore.Domain.Core.Dtos;
using OnlineStore.Infra.Database;

namespace OnlineStore.Infra.Repository
{
    public class OrderItemRepository(AppDbContext context) : IOrderItemRepository
    {
        public async Task<List<OrderItemDto>> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken)
        {
            return await context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                })
                .ToListAsync(cancellationToken);
        }
    }
}
