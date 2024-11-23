using RestaurantAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Domain.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(Guid id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Guid id);
    }
}
