using Moq;
using RestaurantAPI.Application.Services;
using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces.Repositories;
using RestaurantAPI.Domain.Interfaces.Services;
using RestaurantAPI.Domain.Requests;

namespace RestaurantAPI.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IOrderItemService> _mockOrderItemService;
        private readonly Mock<IMenuItemService> _mockMenuItemService;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockOrderItemService = new Mock<IOrderItemService>();
            _mockMenuItemService = new Mock<IMenuItemService>();

            _orderService = new OrderService(
                _mockCustomerRepository.Object,
                _mockOrderRepository.Object,
                _mockOrderItemService.Object,
                _mockMenuItemService.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), TotalPriceCents = 100, Status = "pending" },
                new Order { Id = Guid.NewGuid(), TotalPriceCents = 200, Status = "completed" }
            };
            _mockOrderRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            _mockOrderRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOrder_WhenOrderExists()
        {
            // Arrange
            var order = new Order { Id = Guid.NewGuid(), TotalPriceCents = 100, Status = "pending" };
            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(order);

            // Act
            var result = await _orderService.GetByIdAsync(order.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.Id, result.Id);
            _mockOrderRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsKeyNotFoundException_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = Guid.NewGuid(); // Capture the GUID before the action
            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync((Order)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => _orderService.GetByIdAsync(orderId));
            Assert.Equal($"Order with ID {orderId} not found.", ex.Message); // Use the same orderId in the exception message
        }

        [Fact]
        public async Task AddAsync_CreatesOrderAndItems()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                customer = new CustomerDto
                {
                    firstName = "John",
                    lastName = "Doe",
                    phoneNumber = "123456789"
                },
                OrderItems = new List<OrderItemDto>
                {
                    new OrderItemDto { MenuItem = "Pizza", Quantity = 2 }
                }
            };

            var menuItem = new MenuItem { Id = Guid.NewGuid(), Name = "Pizza", PriceCents = 5000 };
            _mockMenuItemService.Setup(service => service.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(menuItem);
            _mockCustomerRepository.Setup(repo => repo.GetByPhoneNumber(It.IsAny<string>())).Returns((Customer)null);
            _mockOrderRepository.Setup(repo => repo.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
            _mockOrderItemService.Setup(service => service.AddAsync(It.IsAny<OrderItem>())).Returns(Task.CompletedTask);

            // Act
            var result = await _orderService.AddAsync(orderDto, "user123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10000, result.TotalPriceCents);
            _mockOrderRepository.Verify(repo => repo.AddAsync(It.IsAny<Order>()), Times.Once);
            _mockOrderItemService.Verify(service => service.AddAsync(It.IsAny<OrderItem>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentNullException_WhenOrderDtoIsNull()
        {
            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => _orderService.AddAsync(null, "user123"));
            Assert.Equal("Value cannot be null. (Parameter 'orderDto')", ex.Message);
        }

        [Fact]
        public async Task AddItemToOrderAsync_AddsItemToOrder()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var orderItemRequest = new AddOrderItemRequest
            {
                orderItem = new OrderItemDto { MenuItem = "Pizza", Quantity = 3 }
            };

            var order = new Order { Id = orderId, TotalPriceCents = 5000 };
            var menuItem = new MenuItem { Id = Guid.NewGuid(), Name = "Pizza", PriceCents = 5000 };

            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(order);
            _mockMenuItemService.Setup(service => service.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(menuItem);
            _mockOrderRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
            _mockOrderItemService.Setup(service => service.AddAsync(It.IsAny<OrderItem>())).Returns(Task.CompletedTask);

            // Act
            await _orderService.AddItemToOrderAsync(orderId, orderItemRequest);

            // Assert
            Assert.Equal(20000, order.TotalPriceCents);
            _mockOrderItemService.Verify(service => service.AddAsync(It.IsAny<OrderItem>()), Times.Once);
            _mockOrderRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task RemoveItemFromOrderAsync_RemovesItemAndUpdatesTotalPrice()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                TotalPriceCents = 10000,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Id = itemId,
                        Quantity = 2,
                        MenuItem = new MenuItem { PriceCents = 5000 }
                    }
                }
            };

            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(order);
            _mockOrderRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            // Act
            await _orderService.RemoveItemFromOrderAsync(orderId, itemId);

            // Assert
            Assert.Equal(0, order.TotalPriceCents);
            _mockOrderRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Order>()), Times.Once);
        }

        [Theory]
        [InlineData("123", "John", "Doe", "123456789")]
        [InlineData("456", "Jane", "Smith", "987654321")]
        public async Task AddAsync_CreatesCustomer_WhenCustomerDoesNotExist(string phoneNumber, string firstName, string lastName, string customerPhoneNumber)
        {
            // Arrange
            var orderDto = new OrderDto
            {
                customer = new CustomerDto
                {
                    firstName = firstName,
                    lastName = lastName,
                    phoneNumber = customerPhoneNumber
                },
                OrderItems = new List<OrderItemDto>
                {
                    new OrderItemDto { MenuItem = "Pizza", Quantity = 2 }
                }
            };

            var menuItem = new MenuItem { Id = Guid.NewGuid(), Name = "Pizza", PriceCents = 5000 };
            _mockMenuItemService.Setup(service => service.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(menuItem);
            _mockCustomerRepository.Setup(repo => repo.GetByPhoneNumber(It.IsAny<string>())).Returns((Customer)null);
            _mockCustomerRepository.Setup(repo => repo.AddAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);
            _mockOrderRepository.Setup(repo => repo.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
            _mockOrderItemService.Setup(service => service.AddAsync(It.IsAny<OrderItem>())).Returns(Task.CompletedTask);

            // Act
            var result = await _orderService.AddAsync(orderDto, "user123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10000, result.TotalPriceCents);
            _mockCustomerRepository.Verify(repo => repo.AddAsync(It.IsAny<Customer>()), Times.Once);
        }
    }
}
