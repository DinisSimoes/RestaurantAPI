using RestaurantAPI.Domain.Entities;

namespace RestaurantAPI.Domain.Interfaces.Repositories
{
    public interface IMenuItemRepository
    {
        Task<IEnumerable<MenuItem>> GetAllAsync();
        Task<MenuItem> GetByIdAsync(Guid id);
        Task<MenuItem> GetByNameAsync(string name);
        Task AddAsync(MenuItem menuItem);
        Task UpdateAsync(MenuItem menuItem);
        Task DeleteAsync(Guid id);
    }
}
