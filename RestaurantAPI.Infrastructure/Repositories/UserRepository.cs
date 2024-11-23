using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _context;

        public UserRepository(DbContext context)    
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Recupera todos os Users
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Set<User>().ToListAsync();
        }

        // Recupera um User por ID
        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Set<User>().FindAsync(id);
        }

        // Recupera um User por email
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
        }

        // Adiciona um novo User
        public async Task AddAsync(User user)
        {
            await _context.Set<User>().AddAsync(user);
            await _context.SaveChangesAsync();
        }

        // Atualiza um User
        public async Task UpdateAsync(User user)
        {
            _context.Set<User>().Update(user);
            await _context.SaveChangesAsync();
        }

        // Deleta um User por ID
        public async Task DeleteAsync(string id)
        {
            var user = await _context.Set<User>().FindAsync(id);
            if (user != null)
            {
                _context.Set<User>().Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
