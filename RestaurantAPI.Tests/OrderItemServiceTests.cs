using Moq;
using RestaurantAPI.Application.Services;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces.Repositories;

namespace RestaurantAPI.Tests
{
    public class OrderItemServiceTests
    {
        private readonly Mock<IOrderItemRepository> _repositoryMock;
        private readonly OrderItemService _service;

        public OrderItemServiceTests()
        {
            _repositoryMock = new Mock<IOrderItemRepository>();
            _service = new OrderItemService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllOrderItems()
        {
            // Arrange
            var orderItems = new List<OrderItem>
        {
            new OrderItem { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Quantity = 1 },
            new OrderItem { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Quantity = 2 }
        };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(orderItems);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderItems.Count, result.Count());
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOrderItem_WhenExists()
        {
            // Arrange
            var orderItem = new OrderItem { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Quantity = 1 };
            _repositoryMock.Setup(r => r.GetByIdAsync(orderItem.Id)).ReturnsAsync(orderItem);

            // Act
            var result = await _service.GetByIdAsync(orderItem.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderItem.Id, result.Id);
            _repositoryMock.Verify(r => r.GetByIdAsync(orderItem.Id), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsKeyNotFoundException_WhenNotExists()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByIdAsync(invalidId)).ReturnsAsync((OrderItem)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdAsync(invalidId));
            _repositoryMock.Verify(r => r.GetByIdAsync(invalidId), Times.Once);
        }

        [Fact]
        public async Task AddAsync_AddsOrderItem_WhenValid()
        {
            // Arrange
            var orderItem = new OrderItem { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Quantity = 1 };

            // Act
            await _service.AddAsync(orderItem);

            // Assert
            _repositoryMock.Verify(r => r.AddAsync(orderItem), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentNullException_WhenOrderItemIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddAsync(null));
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<OrderItem>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesOrderItem_WhenExists()
        {
            // Arrange
            var existingOrderItem = new OrderItem { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Quantity = 1 };
            var updatedOrderItem = new OrderItem { Id = existingOrderItem.Id, Quantity = 5 };

            _repositoryMock.Setup(r => r.GetByIdAsync(existingOrderItem.Id)).ReturnsAsync(existingOrderItem);

            // Act
            await _service.UpdateAsync(updatedOrderItem);

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<OrderItem>(o =>
                o.Id == existingOrderItem.Id &&
                o.Quantity == updatedOrderItem.Quantity)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenNotExists()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var updatedOrderItem = new OrderItem { Id = nonExistentId, Quantity = 3 };

            _repositoryMock.Setup(r => r.GetByIdAsync(nonExistentId)).ReturnsAsync((OrderItem)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(updatedOrderItem));
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<OrderItem>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_DeletesOrderItem_WhenExists()
        {
            // Arrange
            var existingOrderItem = new OrderItem { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Quantity = 1 };
            _repositoryMock.Setup(r => r.GetByIdAsync(existingOrderItem.Id)).ReturnsAsync(existingOrderItem);

            // Act
            await _service.DeleteAsync(existingOrderItem.Id);

            // Assert
            _repositoryMock.Verify(r => r.DeleteAsync(existingOrderItem.Id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenNotExists()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByIdAsync(invalidId)).ReturnsAsync((OrderItem)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(invalidId));
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public async Task GetByOrderIdAsync_ReturnsOrderItems_WhenExists(int quantity)
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var orderItems = new List<OrderItem>();

            for (int i = 0; i < quantity; i++)
            {
                orderItems.Add(new OrderItem { Id = Guid.NewGuid(), OrderId = orderId, Quantity = i + 1 });
            }

            _repositoryMock.Setup(r => r.GetByOrderIdAsync(orderId)).ReturnsAsync(orderItems);

            // Act
            var result = await _service.GetByOrderIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(quantity, result.Count());
            _repositoryMock.Verify(r => r.GetByOrderIdAsync(orderId), Times.Once);
        }
    }
}
