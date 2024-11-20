using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // Método para obter todos os pedidos
        public async Task<IEnumerable<Order>> GetAllAsync() => await _repository.GetAllAsync();

        // Método para obter um pedido por ID
        public async Task<Order> GetByIdAsync(Guid id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");

            return order;
        }

        // Método para adicionar um novo pedido
        public async Task AddAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            order.Id = Guid.NewGuid(); // Atribuindo novo GUID ao pedido
            await _repository.AddAsync(order);
        }

        // Método para atualizar um pedido existente
        public async Task UpdateAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var existingOrder = await _repository.GetByIdAsync(order.Id);
            if (existingOrder == null)
                throw new KeyNotFoundException($"Order with ID {order.Id} not found.");

            // Atualiza os dados do pedido com as novas informações
            existingOrder.Status = order.Status;
            //TODO: ver isto mais tarde
            //existingOrder.Items = order.Items; // Exemplo de atualização dos itens

            await _repository.UpdateAsync(existingOrder);
        }

        // Método para deletar um pedido
        public async Task DeleteAsync(Guid id)
        {
            var existingOrder = await _repository.GetByIdAsync(id);
            if (existingOrder == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");

            await _repository.DeleteAsync(id);
        }
    }
}
