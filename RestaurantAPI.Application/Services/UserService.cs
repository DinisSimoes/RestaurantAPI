using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // Recupera todos os Users
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // Recupera um User por ID
        public async Task<User> GetByIdAsync(string id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            return user;
        }

        // Recupera um User por email
        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _repository.GetByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException($"User with email {email} not found.");

            return user;
        }

        // Adiciona um novo User
        public async Task AddAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            await _repository.AddAsync(user);
        }

        // Atualiza um User
        public async Task UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existingUser = await _repository.GetByIdAsync(user.Id);
            if (existingUser == null)
                throw new KeyNotFoundException($"User with ID {user.Id} not found.");

            existingUser.FirstName = user.FirstName;
            existingUser.Email = user.Email;

            await _repository.UpdateAsync(existingUser);
        }

        // Deleta um User por ID
        public async Task DeleteAsync(string id)
        {
            var existingUser = await _repository.GetByIdAsync(id);
            if (existingUser == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            await _repository.DeleteAsync(id);
        }
    }
}
