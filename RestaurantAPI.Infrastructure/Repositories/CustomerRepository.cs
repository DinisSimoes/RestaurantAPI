using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces.Repositories;
using RestaurantAPI.Infrastructure.Data;

namespace RestaurantAPI.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Set<Customer>().ToListAsync();
        }

        public IQueryable<Customer> GetAll()
        {
            return _context.Set<Customer>().AsQueryable();
        }

        public async Task<Customer> GetByIdAsync(Guid id)
        {
            return await _context.Set<Customer>().FindAsync(id);
        }

        public Customer? GetByPhoneNumber(string phone)
        {
            return _context.Customers.FirstOrDefault(c => c.PhoneNumber == phone);
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Set<Customer>().AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.Set<Customer>().Update(customer);
            await _context.SaveChangesAsync();
        }

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
