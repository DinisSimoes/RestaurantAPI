using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces;
using RestaurantAPI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Infrastructure.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly DbContext _context;

        public MenuItemRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Recupera todos os itens do menu
        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await _context.Set<MenuItem>().ToListAsync();
        }

        // Recupera um item do menu pelo ID
        public async Task<MenuItem> GetByIdAsync(Guid id)
        {
            return await _context.Set<MenuItem>().FindAsync(id);
        }

        // Adiciona um novo item ao menu
        public async Task AddAsync(MenuItem menuItem)
        {

            await _context.Set<MenuItem>().AddAsync(menuItem);
            await _context.SaveChangesAsync();
        }

        // Atualiza um item do menu
        public async Task UpdateAsync(MenuItem menuItem)
        {
            _context.Set<MenuItem>().Update(menuItem);
            await _context.SaveChangesAsync();
        }

        // Deleta um item do menu
        public async Task DeleteAsync(Guid id)
        {
            var menuItem = await _context.Set<MenuItem>().FindAsync(id);
            if (menuItem != null)
            {
                _context.Set<MenuItem>().Remove(menuItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
