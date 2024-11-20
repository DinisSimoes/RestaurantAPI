using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // Recupera todos os Customers
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // Recupera um Customer por ID
        public async Task<Customer> GetByIdAsync(Guid id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {id} not found.");

            return customer;
        }

        // Recupera um Customer por telefone
        public async Task<Customer> GetByPhoneAsync(string phone)
        {
            var customer = await _repository.GetByPhoneAsync(phone);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with phone {phone} not found.");

            return customer;
        }

        // Adiciona um novo Customer
        public async Task AddAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            await _repository.AddAsync(customer);
        }

        // Atualiza um Customer
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

        // Deleta um Customer por ID
        public async Task DeleteAsync(Guid id)
        {
            var existingCustomer = await _repository.GetByIdAsync(id);
            if (existingCustomer == null)
                throw new KeyNotFoundException($"Customer with ID {id} not found.");

            await _repository.DeleteAsync(id);
        }
    }
}
