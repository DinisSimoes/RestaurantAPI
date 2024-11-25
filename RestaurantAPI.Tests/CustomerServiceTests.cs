using Moq;
using RestaurantAPI.Application.Services;
using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces.Repositories;

namespace RestaurantAPI.Tests
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _repositoryMock;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _repositoryMock = new Mock<ICustomerRepository>();
            _customerService = new CustomerService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsCustomers_WhenCustomersExist()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", PhoneNumber = "1234567890" },
                new Customer { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Doe", PhoneNumber = "0987654321" }
            };

            _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(customers);

            // Act
            var result = await _customerService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsKeyNotFoundException_WhenCustomerNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(customerId)).ReturnsAsync((Customer)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _customerService.GetByIdAsync(customerId));
            Assert.Equal($"Customer with ID {customerId} not found.", exception.Message);
        }

        [Fact]
        public async Task GetByPhoneAsync_ReturnsCustomer_WhenCustomerExists()
        {
            // Arrange
            var phone = "1234567890";
            var customer = new Customer { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", PhoneNumber = phone };

            _repositoryMock.Setup(repo => repo.GetByPhoneNumber(phone)).Returns(customer);

            // Act
            var result = await _customerService.GetByPhoneAsync(phone);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(phone, result.PhoneNumber);
        }

        [Fact]
        public async Task GetByPhoneAsync_ThrowsKeyNotFoundException_WhenCustomerNotFound()
        {
            // Arrange
            var phone = "1234567890";

            _repositoryMock.Setup(repo => repo.GetByPhoneNumber(phone)).Returns((Customer)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _customerService.GetByPhoneAsync(phone));
            Assert.Equal($"Customer with phone {phone} not found.", exception.Message);
        }

        [Fact]
        public async Task AddAsync_AddsCustomer_WhenCustomerIsValid()
        {
            // Arrange
            var customerDto = new CustomerDto
            {
                firstName = "John",
                lastName = "Doe",
                phoneNumber = "1234567890"
            };

            // Act
            await _customerService.AddAsync(customerDto);

            // Assert
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentNullException_WhenCustomerDtoIsNull()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _customerService.AddAsync(null));
            Assert.Equal("customerDto", exception.ParamName);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenCustomerNotFound()
        {
            // Arrange
            var customer = new Customer { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", PhoneNumber = "1234567890" };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(customer.Id)).ReturnsAsync((Customer)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _customerService.UpdateAsync(customer));
            Assert.Equal($"Customer with ID {customer.Id} not found.", exception.Message);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesCustomer_WhenCustomerExists()
        {
            // Arrange
            var customer = new Customer { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", PhoneNumber = "1234567890" };
            var existingCustomer = new Customer { Id = customer.Id, FirstName = "John", LastName = "Doe", PhoneNumber = "1234567890" };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(customer.Id)).ReturnsAsync(existingCustomer);

            // Act
            await _customerService.UpdateAsync(customer);

            // Assert
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Customer>(c => c.Id == customer.Id)), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenCustomerNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(customerId)).ReturnsAsync((Customer)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _customerService.DeleteAsync(customerId));
            Assert.Equal($"Customer with ID {customerId} not found.", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_DeletesCustomer_WhenCustomerExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer { Id = customerId, FirstName = "John", LastName = "Doe", PhoneNumber = "1234567890" };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(customerId)).ReturnsAsync(customer);

            // Act
            await _customerService.DeleteAsync(customerId);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(customerId), Times.Once);
        }
    }
}
