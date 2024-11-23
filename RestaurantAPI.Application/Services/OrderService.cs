using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces.Repositories;
using RestaurantAPI.Domain.Interfaces.Services;
using RestaurantAPI.Domain.Requests;

namespace RestaurantAPI.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemService _orderItemService;
        private readonly IMenuItemService _menuItemService;

        public OrderService(ICustomerRepository customerRepository, IOrderRepository orderRepository, IOrderItemService orderItemService, IMenuItemService menuItemService)
        {
            _customerRepository = customerRepository;
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
            ValidateOrderDto(orderDto);

            var customer = _customerRepository.GetByPhoneNumber(orderDto.customer.phoneNumber);

            if (customer == null)
            {
                ValidateCustomerDto(orderDto.customer);
                customer = new Customer
                {
                    FirstName = orderDto.customer.firstName,
                    LastName = orderDto.customer.lastName,
                    PhoneNumber = orderDto.customer.phoneNumber,
                };

                await _customerRepository.AddAsync(customer);
            }

            var total = 0;

            foreach (var t in orderDto.OrderItems)
            {
                MenuItem menuItem = await _menuItemService.GetByNameAsync(t.MenuItem);
                var result = menuItem.PriceCents * t.Quantity;
                total = total + result;
            }

            var order = new Order
            {
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

                foreach (var t in orderDto.OrderItems)
                {
                    MenuItem menuItem = await _menuItemService.GetByNameAsync(t.MenuItem);

                    OrderItem item = new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        Order = order,
                        ItemId = menuItem.Id,
                        MenuItem = menuItem,
                        Quantity = t.Quantity
                    };

                    await _orderItemService.AddAsync(item);
                }
                return order;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddItemToOrderAsync(Guid orderId, AddOrderItemRequest request)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");

            var menuItem = await _menuItemService.GetByNameAsync(request.orderItem.MenuItem);
            if (menuItem == null)
                throw new KeyNotFoundException($"MenuItem with name {request.orderItem.MenuItem} not found.");

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ItemId = menuItem.Id,
                Quantity = request.orderItem.Quantity
            };

            order.TotalPriceCents += menuItem.PriceCents * orderItem.Quantity;

            await _orderItemService.AddAsync(orderItem);
            await _orderRepository.UpdateAsync(order);
        }

        private void ValidateOrderDto(OrderDto orderDto)
        {
            if (orderDto == null) throw new ArgumentNullException(nameof(orderDto));
            if (orderDto.customer == null) throw new ArgumentException("Customer information is required.");
        }

        private void ValidateCustomerDto(CustomerDto customerDto)
        {
            if (string.IsNullOrWhiteSpace(customerDto.firstName)) throw new ArgumentException("First name is required.");
            if (string.IsNullOrWhiteSpace(customerDto.lastName)) throw new ArgumentException("Last name is required.");
            if (string.IsNullOrWhiteSpace(customerDto.phoneNumber)) throw new ArgumentException("Phone number is required.");
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
