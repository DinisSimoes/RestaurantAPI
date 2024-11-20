using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _repository;

        public OrderItemService(IOrderItemRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // Recupera todos os OrderItems
        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // Recupera um OrderItem por ID
        public async Task<OrderItem> GetByIdAsync(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException($"OrderItem with ID {id} not found.");

            return item;
        }

        // Adiciona um novo OrderItem
        public async Task AddAsync(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            await _repository.AddAsync(orderItem);
        }

        // Atualiza um OrderItem
        public async Task UpdateAsync(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            var existingOrderItem = await _repository.GetByIdAsync(orderItem.Id);
            if (existingOrderItem == null)
                throw new KeyNotFoundException($"OrderItem with ID {orderItem.Id} not found.");

            //existingOrderItem.Name = orderItem.Name;
            existingOrderItem.Quantity = orderItem.Quantity;
            //existingOrderItem.PriceCents = orderItem.PriceCents;

            await _repository.UpdateAsync(existingOrderItem);
        }

        // Deleta um OrderItem
        public async Task DeleteAsync(Guid id)
        {
            var existingOrderItem = await _repository.GetByIdAsync(id);
            if (existingOrderItem == null)
                throw new KeyNotFoundException($"OrderItem with ID {id} not found.");

            await _repository.DeleteAsync(id);
        }

        // Recupera todos os itens de um pedido
        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId)
        {
            return await _repository.GetByOrderIdAsync(orderId);
        }
    }
}
