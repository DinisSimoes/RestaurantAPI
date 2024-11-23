using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces.Repositories;
using RestaurantAPI.Domain.Interfaces.Services;

namespace RestaurantAPI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _repository.GetByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException($"User with email {email} not found.");

            return user;
        }

        public async Task AddAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            await _repository.AddAsync(user);
        }

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

        public async Task DeleteAsync(string id)
        {
            var existingUser = await _repository.GetByIdAsync(id);
            if (existingUser == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            await _repository.DeleteAsync(id);
        }
    }
}
