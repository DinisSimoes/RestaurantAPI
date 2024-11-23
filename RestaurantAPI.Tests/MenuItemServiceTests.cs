using Moq;
using RestaurantAPI.Application.Services;
using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces.Repositories;

namespace RestaurantAPI.Tests
{
    public class MenuItemServiceTests
    {
        private readonly Mock<IMenuItemRepository> _repositoryMock;
        private readonly MenuItemService _service;

        public MenuItemServiceTests()
        {
            _repositoryMock = new Mock<IMenuItemRepository>();
            _service = new MenuItemService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllMenuItems()
        {
            // Arrange
            var menuItems = new List<MenuItem>
            {
                new MenuItem { Id = Guid.NewGuid(), Name = "Pizza", PriceCents = 1500 },
                new MenuItem { Id = Guid.NewGuid(), Name = "Burger", PriceCents = 1200 }
            };
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(menuItems);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(menuItems.Count, result.Count());
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsMenuItem()
        {
            // Arrange
            var menuItem = new MenuItem { Id = Guid.NewGuid(), Name = "Pizza", PriceCents = 1500 };
            _repositoryMock.Setup(r => r.GetByIdAsync(menuItem.Id)).ReturnsAsync(menuItem);

            // Act
            var result = await _service.GetByIdAsync(menuItem.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(menuItem.Name, result.Name);
            _repositoryMock.Verify(r => r.GetByIdAsync(menuItem.Id), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByIdAsync(nonExistingId)).ReturnsAsync((MenuItem)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdAsync(nonExistingId));
            _repositoryMock.Verify(r => r.GetByIdAsync(nonExistingId), Times.Once);
        }

        [Theory]
        [InlineData("Pizza", 1500)]
        [InlineData("Burger", 1200)]
        public async Task GetByNameAsync_ExistingName_ReturnsMenuItem(string name, int price)
        {
            // Arrange
            var menuItem = new MenuItem { Id = Guid.NewGuid(), Name = name, PriceCents = price };
            _repositoryMock.Setup(r => r.GetByNameAsync(name)).ReturnsAsync(menuItem);

            // Act
            var result = await _service.GetByNameAsync(name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
            _repositoryMock.Verify(r => r.GetByNameAsync(name), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ValidMenuItem_AddsMenuItem()
        {
            // Arrange
            var menuItemDto = new MenuItemDto { Name = "Pizza", PriceCents = 1500 };
            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<MenuItem>())).Returns(Task.CompletedTask);

            // Act
            await _service.AddAsync(menuItemDto);

            // Assert
            _repositoryMock.Verify(r => r.AddAsync(It.Is<MenuItem>(m => m.Name == menuItemDto.Name && m.PriceCents == menuItemDto.PriceCents)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ValidId_UpdatesMenuItem()
        {
            // Arrange
            var existingMenuItem = new MenuItem { Id = Guid.NewGuid(), Name = "Pizza", PriceCents = 1500 };
            var menuItemDto = new MenuItemDto { Name = "Updated Pizza", PriceCents = 1600 };
            _repositoryMock.Setup(r => r.GetByIdAsync(existingMenuItem.Id)).ReturnsAsync(existingMenuItem);
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<MenuItem>())).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(existingMenuItem.Id, menuItemDto);

            // Assert
            Assert.Equal(menuItemDto.Name, existingMenuItem.Name);
            Assert.Equal(menuItemDto.PriceCents, existingMenuItem.PriceCents);
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<MenuItem>(m => m.Name == menuItemDto.Name && m.PriceCents == menuItemDto.PriceCents)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            var menuItemDto = new MenuItemDto { Name = "Updated Pizza", PriceCents = 1600 };
            _repositoryMock.Setup(r => r.GetByIdAsync(nonExistingId)).ReturnsAsync((MenuItem)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(nonExistingId, menuItemDto));
            _repositoryMock.Verify(r => r.GetByIdAsync(nonExistingId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ValidId_DeletesMenuItem()
        {
            // Arrange
            var menuItem = new MenuItem { Id = Guid.NewGuid(), Name = "Pizza", PriceCents = 1500 };
            _repositoryMock.Setup(r => r.GetByIdAsync(menuItem.Id)).ReturnsAsync(menuItem);
            _repositoryMock.Setup(r => r.DeleteAsync(menuItem.Id)).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAsync(menuItem.Id);

            // Assert
            _repositoryMock.Verify(r => r.DeleteAsync(menuItem.Id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByIdAsync(nonExistingId)).ReturnsAsync((MenuItem)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(nonExistingId));
            _repositoryMock.Verify(r => r.GetByIdAsync(nonExistingId), Times.Once);
        }
    }
}
