using RestaurantAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Domain.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task<OrderItem> GetByIdAsync(Guid id);
        Task AddAsync(OrderItem orderItem);
        Task UpdateAsync(OrderItem orderItem);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId);
    }
}
