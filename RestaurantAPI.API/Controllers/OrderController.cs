using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Interfaces.Services;
using RestaurantAPI.Domain.Requests;
using System.Security.Claims;

namespace RestaurantAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(IOrderService orderService, IHttpContextAccessor httpContextAccessor)
        {
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Cria um novo pedido.
        /// </summary>
        /// <param name="orderDto">Dados do pedido a ser criado.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        /// <response code="200">Pedido criado com sucesso.</response>
        /// <response code="400">Dados do pedido são inválidos ou faltando.</response>
        /// <response code="500">Erro interno ao criar o pedido.</response>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            if (orderDto == null) return BadRequest(new { Message = "Order data is required." });

            try
            {
                var userId = GetUserFromClaims();

                var order = await _orderService.AddAsync(orderDto, userId);
                return Ok(new { Message = "Order created successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the order.", Details = ex.Message });
            }

        }

        /// <summary>
        /// Atualiza o status de um pedido.
        /// </summary>
        /// <param name="id">ID do pedido.</param>
        /// <param name="request">Dados do novo status para o pedido.</param>
        /// <returns>Resposta de sucesso ou erro.</returns>
        /// <response code="204">Status do pedido atualizado com sucesso.</response>
        /// <response code="404">Pedido não encontrado.</response>
        /// <response code="400">Dados inválidos para atualização.</response>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Cashier")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderRequest request)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            order.Status = request.Status;

            await _orderService.UpdateAsync(order);
            return NoContent();
        }

        /// <summary>
        /// Remove um item de um pedido.
        /// </summary>
        /// <param name="orderId">ID do pedido.</param>
        /// <param name="itemId">ID do item a ser removido.</param>
        /// <returns>Resposta de sucesso ou erro.</returns>
        /// <response code="204">Item removido com sucesso.</response>
        /// <response code="404">Pedido ou item não encontrado.</response>
        /// <response code="500">Erro interno ao remover o item.</response>
        [HttpDelete("{orderId}/items/{itemId}")]
        [Authorize(Roles = "Cashier")]
        public async Task<IActionResult> RemoveItemFromOrder(Guid orderId, Guid itemId)
        {
            try
            {
                await _orderService.RemoveItemFromOrderAsync(orderId, itemId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Adiciona um item a um pedido existente.
        /// </summary>
        /// <param name="orderId">ID do pedido ao qual o item será adicionado.</param>
        /// <param name="request">Dados do item a ser adicionado.</param>
        /// <returns>Resposta de sucesso ou erro.</returns>
        /// <response code="200">Item adicionado com sucesso.</response>
        /// <response code="404">Pedido não encontrado.</response>
        /// <response code="400">Dados inválidos para o item.</response>
        [HttpPost("{orderId}/items")]
        [Authorize(Roles = "Cashier")]
        public async Task<IActionResult> AddItemToOrder(Guid orderId, [FromBody] AddOrderItemRequest request)
        {
            try
            {
                await _orderService.AddItemToOrderAsync(orderId, request);
                return Ok(new { Message = "Item added to the order successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        private string GetUserFromClaims()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown";
        }
    }
}
