using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Models;

namespace RestaurantAPI.Domain.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(Guid id);
        Task<Customer> GetByPhoneAsync(string phone);
        Task<Customer> AddAsync(CustomerDto customerDto);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Guid id);
        Task<PageResult<Customer>> GetClientsAsync(int page, int pageSize);
    }
}
