using RestaurantAPI.Domain.Entities;

namespace RestaurantAPI.Domain.Interfaces.Services
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task<OrderItem> GetByIdAsync(Guid id);
        Task AddAsync(OrderItem orderItem);
        Task UpdateAsync(OrderItem orderItem);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId);
    }
}
