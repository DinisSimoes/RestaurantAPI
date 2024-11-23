using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces.Repositories;
using RestaurantAPI.Domain.Interfaces.Services;

namespace RestaurantAPI.Application.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _repository;

        public OrderItemService(IOrderItemRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<OrderItem> GetByIdAsync(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException($"OrderItem with ID {id} not found.");

            return item;
        }

        public async Task AddAsync(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            await _repository.AddAsync(orderItem);
        }

        public async Task UpdateAsync(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            var existingOrderItem = await _repository.GetByIdAsync(orderItem.Id);
            if (existingOrderItem == null)
                throw new KeyNotFoundException($"OrderItem with ID {orderItem.Id} not found.");

            existingOrderItem.Quantity = orderItem.Quantity;

            await _repository.UpdateAsync(existingOrderItem);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingOrderItem = await _repository.GetByIdAsync(id);
            if (existingOrderItem == null)
                throw new KeyNotFoundException($"OrderItem with ID {id} not found.");

            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId)
        {
            return await _repository.GetByOrderIdAsync(orderId);
        }
    }
}
