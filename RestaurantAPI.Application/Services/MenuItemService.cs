using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces.Repositories;
using RestaurantAPI.Domain.Interfaces.Services;

namespace RestaurantAPI.Application.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _repository;

        public MenuItemService(IMenuItemRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<MenuItemDto> GetByIdAsync(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException($"MenuItem with ID {id} not found.");

            return new MenuItemDto
            {
                Name = item.Name,
                PriceCents = item.PriceCents,
            };
        }

        public async Task<MenuItem> GetByNameAsync(string name)
        {
            var item = await _repository.GetByNameAsync(name);
            if (item == null)
                throw new KeyNotFoundException($"MenuItem with name {name} not found.");

            return item;
        }

        public async Task AddAsync(MenuItemDto menuItemDto)
        {
            if (menuItemDto == null)
                throw new ArgumentNullException(nameof(menuItemDto));

            MenuItem menuItem = new MenuItem
            {
                Id = Guid.NewGuid(),
                Name = menuItemDto.Name,
                PriceCents = menuItemDto.PriceCents
            };

            await _repository.AddAsync(menuItem);
        }

        public async Task UpdateAsync(Guid id, MenuItemDto menuItemDto)
        {
            if (menuItemDto == null)
                throw new ArgumentNullException(nameof(menuItemDto));

            var existingMenuItem = await _repository.GetByIdAsync(id);
            if (existingMenuItem == null)
                throw new KeyNotFoundException($"MenuItem with ID {id} not found.");

            existingMenuItem.Name = menuItemDto.Name;
            existingMenuItem.PriceCents = menuItemDto.PriceCents;

            await _repository.UpdateAsync(existingMenuItem);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingMenuItem = await _repository.GetByIdAsync(id);
            if (existingMenuItem == null)
                throw new KeyNotFoundException($"MenuItem with ID {id} not found.");

            await _repository.DeleteAsync(id);
        }
    }
}
