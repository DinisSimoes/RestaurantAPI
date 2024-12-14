using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Entities;

namespace RestaurantAPI.Domain.Interfaces.Services
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItem>> GetAllAsync();
        Task<MenuItemDto> GetByIdAsync(Guid id);
        Task<MenuItem> GetByNameAsync(string name);
        Task AddAsync(MenuItemDto menuItemDto);
        Task UpdateAsync(Guid id, MenuItemDto menuItem);
        Task DeleteAsync(Guid id);
    }
}
