using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _repository;

        public MenuItemService(IMenuItemRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // Recupera todos os MenuItems
        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // Recupera um MenuItem por ID
        public async Task<MenuItem> GetByIdAsync(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException($"MenuItem with ID {id} not found.");

            return item;
        }

        // Adiciona um novo MenuItem
        public async Task AddAsync(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentNullException(nameof(menuItem));

            await _repository.AddAsync(menuItem);
        }

        // Atualiza um MenuItem
        public async Task UpdateAsync(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentNullException(nameof(menuItem));

            var existingMenuItem = await _repository.GetByIdAsync(menuItem.Id);
            if (existingMenuItem == null)
                throw new KeyNotFoundException($"MenuItem with ID {menuItem.Id} not found.");

            existingMenuItem.Name = menuItem.Name;
            existingMenuItem.PriceCents = menuItem.PriceCents;

            await _repository.UpdateAsync(existingMenuItem);
        }

        // Deleta um MenuItem
        public async Task DeleteAsync(Guid id)
        {
            var existingMenuItem = await _repository.GetByIdAsync(id);
            if (existingMenuItem == null)
                throw new KeyNotFoundException($"MenuItem with ID {id} not found.");

            await _repository.DeleteAsync(id);
        }
    }
}
