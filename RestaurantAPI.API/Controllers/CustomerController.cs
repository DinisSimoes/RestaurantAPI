using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Interfaces.Services;

namespace RestaurantAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Authorize(Roles = "Cashier")]
        public async Task<IActionResult> GetClients([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var clients = await _customerService.GetClientsAsync(page, pageSize);
            return Ok(clients);
        }

        [HttpPost]
        [Authorize(Roles = "Cashier")]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerDto customerDto)
        {
            if (
                customerDto == null || 
                string.IsNullOrWhiteSpace(customerDto.firstName) || 
                string.IsNullOrWhiteSpace(customerDto.lastName) || 
                string.IsNullOrWhiteSpace(customerDto.phoneNumber)
                )
            {
                return BadRequest("Invalid customer data.");
            }

            await _customerService.AddAsync(customerDto);

            return Ok(new
            {
                Message = "Customer added successfully.",
                Customer = customerDto
            });
        }

    }
}
