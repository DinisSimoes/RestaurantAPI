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

        [HttpPost]
        [Authorize(Roles = "Cashier")]
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

        [HttpPut("{id}")]
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

        [HttpDelete("orders/{orderId}/items/{itemId}")]
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

        [HttpPost("addItems/{orderId}")]
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
