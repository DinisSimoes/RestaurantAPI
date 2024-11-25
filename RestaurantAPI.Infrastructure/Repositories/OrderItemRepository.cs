using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces.Repositories;
using RestaurantAPI.Infrastructure.Data;

namespace RestaurantAPI.Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _context;

        public OrderItemRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _context.Set<OrderItem>().ToListAsync();
        }

        public async Task<OrderItem> GetByIdAsync(Guid id)
        {
            return await _context.Set<OrderItem>().FindAsync(id);
        }

        public async Task AddAsync(OrderItem orderItem)
        {
            await _context.Set<OrderItem>().AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderItem orderItem)
        {
            _context.Set<OrderItem>().Update(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var orderItem = await _context.Set<OrderItem>().FindAsync(id);
            if (orderItem != null)
            {
                _context.Set<OrderItem>().Remove(orderItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId)
        {
            return await _context.Set<OrderItem>()
                                 .Where(oi => oi.OrderId == orderId)
                                 .ToListAsync();
        }
    }
}
