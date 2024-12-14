using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces.Repositories;
using RestaurantAPI.Domain.Interfaces.Services;
using RestaurantAPI.Domain.Models;

namespace RestaurantAPI.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Customer> GetByIdAsync(Guid id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {id} not found.");

            return customer;
        }

        public async Task<Customer> GetByPhoneAsync(string phone)
        {
            var customer = _repository.GetByPhoneNumber(phone);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with phone {phone} not found.");

            return customer;
        }

        public async Task AddAsync(CustomerDto customerDto)
        {
            if (customerDto == null)
                throw new ArgumentNullException(nameof(customerDto));

            Customer costumer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = customerDto.firstName,
                LastName = customerDto.lastName,
                PhoneNumber = customerDto.phoneNumber,
            };

            await _repository.AddAsync(costumer);
        }

        public async Task UpdateAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var existingCustomer = await _repository.GetByIdAsync(customer.Id);
            if (existingCustomer == null)
                throw new KeyNotFoundException($"Customer with ID {customer.Id} not found.");

            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.PhoneNumber = customer.PhoneNumber;

            await _repository.UpdateAsync(existingCustomer);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingCustomer = await _repository.GetByIdAsync(id);
            if (existingCustomer == null)
                throw new KeyNotFoundException($"Customer with ID {id} not found.");

            await _repository.DeleteAsync(id);
        }

        public async Task<PageResult<Customer>> GetCustomersAsync(int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var skip = (page - 1) * pageSize;

            var costumersQuery = _repository.GetAll();

            var totalCostumers = await costumersQuery.CountAsync();

            var costumers = await costumersQuery
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PageResult<Customer>
            {
                Items = costumers,
                TotalItems = totalCostumers,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
