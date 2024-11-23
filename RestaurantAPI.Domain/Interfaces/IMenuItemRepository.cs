using RestaurantAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Domain.Interfaces
{
    public interface IMenuItemRepository
    {
        Task<IEnumerable<MenuItem>> GetAllAsync(); 
        Task<MenuItem> GetByIdAsync(Guid id); 
        Task AddAsync(MenuItem menuItem);
        Task UpdateAsync(MenuItem menuItem); 
        Task DeleteAsync(Guid id);
    }
}
