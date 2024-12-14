
using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Entities;

namespace RestaurantAPI.Domain.Interfaces.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(Guid id);
        Task<Order> AddAsync(OrderDto orderDto, string userId);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Guid id);
        Task AddItemToOrderAsync(Guid orderId, OrderItemDto request);
        Task RemoveItemFromOrderAsync(Guid orderId, Guid itemId);
    }
}
