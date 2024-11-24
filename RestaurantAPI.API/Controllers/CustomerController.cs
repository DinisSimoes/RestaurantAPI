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

        /// <summary>
        /// Obtém uma lista de clientes com paginação.
        /// </summary>
        /// <param name="page">Número da página a ser recuperada (padrão é 1).</param>
        /// <param name="pageSize">Número de clientes por página (padrão é 10).</param>
        /// <returns>Lista de clientes com base na paginação fornecida.</returns>
        /// <response code="200">Lista de clientes retornada com sucesso.</response>
        [HttpGet]
        [Authorize(Roles = "Cashier")]
        public async Task<IActionResult> GetClients([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var clients = await _customerService.GetClientsAsync(page, pageSize);
            return Ok(clients);
        }

        /// <summary>
        /// Adiciona um novo cliente ao sistema.
        /// </summary>
        /// <param name="customerDto">Dados do cliente a ser adicionado.</param>
        /// <returns>Mensagem de sucesso e dados do cliente criado.</returns>
        /// <response code="200">Cliente adicionado com sucesso.</response>
        /// <response code="400">Dados do cliente inválidos ou faltando.</response>
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
