using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContext _context;

        public OrderRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Método para obter todos os pedidos
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Set<Order>().ToListAsync();
        }

        // Método para obter um pedido por ID
        public async Task<Order> GetByIdAsync(Guid id)
        {
            return await _context.Set<Order>().FindAsync(id);
        }

        // Método para adicionar um novo pedido
        public async Task AddAsync(Order order)
        {
            await _context.Set<Order>().AddAsync(order);
            await _context.SaveChangesAsync();
        }

        // Método para atualizar um pedido existente
        public async Task UpdateAsync(Order order)
        {
            _context.Set<Order>().Update(order);
            await _context.SaveChangesAsync();
        }

        // Método para deletar um pedido
        public async Task DeleteAsync(Guid id)
        {
            var order = await _context.Set<Order>().FindAsync(id);
            if (order != null)
            {
                _context.Set<Order>().Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
