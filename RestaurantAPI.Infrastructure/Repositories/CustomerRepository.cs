using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DbContext _context;

        public CustomerRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Recupera todos os Customers
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Set<Customer>().ToListAsync();
        }

        // Recupera um Customer por ID
        public async Task<Customer> GetByIdAsync(Guid id)
        {
            return await _context.Set<Customer>().FindAsync(id);
        }

        // Recupera um Customer por telefone
        public async Task<Customer> GetByPhoneAsync(string phone)
        {
            return await _context.Set<Customer>().FirstOrDefaultAsync(c => c.PhoneNumber == phone);
        }

        // Adiciona um novo Customer
        public async Task AddAsync(Customer customer)
        {
            await _context.Set<Customer>().AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        // Atualiza um Customer
        public async Task UpdateAsync(Customer customer)
        {
            _context.Set<Customer>().Update(customer);
            await _context.SaveChangesAsync();
        }

        // Deleta um Customer por ID
        public async Task DeleteAsync(Guid id)
        {
            var customer = await _context.Set<Customer>().FindAsync(id);
            if (customer != null)
            {
                _context.Set<Customer>().Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
