using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly DbContext _context;

        public OrderItemRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Recupera todos os OrderItems
        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _context.Set<OrderItem>().ToListAsync();
        }

        // Recupera um OrderItem por ID
        public async Task<OrderItem> GetByIdAsync(Guid id)
        {
            return await _context.Set<OrderItem>().FindAsync(id);
        }

        // Adiciona um novo OrderItem
        public async Task AddAsync(OrderItem orderItem)
        {
            await _context.Set<OrderItem>().AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }

        // Atualiza um OrderItem
        public async Task UpdateAsync(OrderItem orderItem)
        {
            _context.Set<OrderItem>().Update(orderItem);
            await _context.SaveChangesAsync();
        }

        // Deleta um OrderItem por ID
        public async Task DeleteAsync(Guid id)
        {
            var orderItem = await _context.Set<OrderItem>().FindAsync(id);
            if (orderItem != null)
            {
                _context.Set<OrderItem>().Remove(orderItem);
                await _context.SaveChangesAsync();
            }
        }

        // Recupera todos os OrderItems relacionados a um Order
        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId)
        {
            return await _context.Set<OrderItem>()
                                 .Where(oi => oi.OrderId == orderId)
                                 .ToListAsync();
        }
    }
}
