using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces.Repositories;
using RestaurantAPI.Domain.Interfaces.Services;
using RestaurantAPI.Domain.Requests;

namespace RestaurantAPI.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemService _orderItemService;
        private readonly IMenuItemService _menuItemService;

        public OrderService(ICustomerService customerService, IOrderRepository orderRepository, IOrderItemService orderItemService, IMenuItemService menuItemService)
        {
            _customerService = customerService;
            _orderRepository = orderRepository;
            _orderItemService = orderItemService;
            _menuItemService = menuItemService;
        }

        public async Task<IEnumerable<Order>> GetAllAsync() => await _orderRepository.GetAllAsync();

        public async Task<Order> GetByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");

            return order;
        }

        public async Task<Order> AddAsync(OrderDto orderDto, string userId)
        {
            Customer customer = await _customerService.GetByPhoneAsync(orderDto.Customer.phoneNumber);

            if (customer == null)
            {
                customer = await _customerService.AddAsync(orderDto.Customer);
            }

            var total = 0;

            var order = new Order
            {
                Id = Guid.NewGuid(),
                Customer = customer,
                TotalPriceCents = total,
                Status = "pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = userId
            };

            try
            {
                await _orderRepository.AddAsync(order);

                foreach (OrderItemDto t in orderDto.OrderItems)
                {
                    MenuItem menuItem = await _menuItemService.GetByNameAsync(t.MenuItem);
                    await AddItemToOrderAsync(order.Id, t);
                }
                return order;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddItemToOrderAsync(Guid orderId, OrderItemDto request)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");

            var menuItem = await _menuItemService.GetByNameAsync(request.MenuItem);
            if (menuItem == null)
                throw new KeyNotFoundException($"MenuItem with name {request.MenuItem} not found.");

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                Order = order,
                ItemId = menuItem.Id,
                MenuItem = menuItem,
                Quantity = request.Quantity
            };

            order.TotalPriceCents += menuItem.PriceCents * orderItem.Quantity;

            await _orderRepository.UpdateAsync(order);
            await _orderItemService.AddAsync(orderItem);

        }

        public async Task UpdateAsync(Order order)
        {
            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");

            await _orderRepository.DeleteAsync(id);
        }

        public async Task RemoveItemFromOrderAsync(Guid orderId, Guid itemId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }

            var item = order.OrderItems.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                throw new KeyNotFoundException($"Item with ID {itemId} not found in order.");
            }

            order.TotalPriceCents -= item.MenuItem.PriceCents * item.Quantity;

            order.OrderItems.Remove(item);

            await _orderRepository.UpdateAsync(order);
        }
    }
}
