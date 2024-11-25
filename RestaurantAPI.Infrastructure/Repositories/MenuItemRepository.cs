using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces.Repositories;
using RestaurantAPI.Infrastructure.Data;

namespace RestaurantAPI.Infrastructure.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly AppDbContext _context;

        public MenuItemRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await _context.Set<MenuItem>().ToListAsync();
        }

        public async Task<MenuItem> GetByIdAsync(Guid id)
        {
            return await _context.Set<MenuItem>().FindAsync(id);
        }

        public async Task<MenuItem?> GetByNameAsync(string name)
        {
            return await _context.Set<MenuItem>()
                .FirstOrDefaultAsync(item => item.Name == name);
        }

        public async Task AddAsync(MenuItem menuItem)
        {

            await _context.Set<MenuItem>().AddAsync(menuItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MenuItem menuItem)
        {
            _context.Set<MenuItem>().Update(menuItem);
            await _context.SaveChangesAsync();
        }

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
