using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Application.Commands.Order.CreateOrder;
using RestaurantAPI.Application.Commands.Order.UpdateOrder;
using RestaurantAPI.Application.Commands.OrderItem.AddItemToOrder;
using RestaurantAPI.Application.Commands.OrderItem.DeleteItemFromOrder;
using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces.Services;
using System.Security.Claims;

namespace RestaurantAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
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
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand request)
        {
            if (request == null)
            {
                return BadRequest(new { Message = "Order data is required." });
            }

            var response = await _mediator.Send(request);

            // Retornar a resposta conforme o sucesso ou falha
            if (response.Success)
            {
                return Ok(new { Message = response.Message });
            }

            return BadRequest(new { Message = response.Message });

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
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderCommand request)
        {
            if (request == null)
            {
                return BadRequest(new { Message = "Request body is required." });
            }

            var response = await _mediator.Send(new UpdateOrderCommand { Id = id, Status = request.Status});

            if (response.Success)
            {
                return NoContent();
            }

            return NotFound(new { Message = response.Message });
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
        public async Task<IActionResult> DeleteItemFromOrder(Guid orderId, Guid itemId)
        {
            var command = new DeleteItemFromOrderCommand
            {
                OrderId = orderId,
                ItemId = itemId
            };

            var response = await _mediator.Send(command);

            if (response.Success)
            {
                return NoContent();
            }

            return response.Message.Contains("not found")
                ? NotFound(new { Message = response.Message })
                : StatusCode(500, new { Message = response.Message });
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
        public async Task<IActionResult> AddItemToOrder(Guid orderId, [FromBody] OrderItemDto orderItem)
        {
            var command = new AddItemToOrderCommand
            {
                OrderId = orderId,
                OrderItem = orderItem
            };

            var response = await _mediator.Send(command);

            if (response.Success)
            {
                return Ok(new { Message = response.Message });
            }

            return response.Message.Contains("not found")
                ? NotFound(new { Message = response.Message })
                : BadRequest(new { Error = response.Message });
        }
    }
}
