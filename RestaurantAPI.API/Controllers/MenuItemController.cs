using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Interfaces.Services;

namespace RestaurantAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuItemById(Guid id)
        {
            var menuItem = await _menuItemService.GetByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound($"MenuItem with ID {id} not found.");
            }

            return Ok(menuItem);
        }

        [HttpPost]
        [Authorize(Roles = "Cashier")]
        public async Task<IActionResult> CreateMenuItem([FromBody] MenuItemDto menuItemDto)
        {
            if (menuItemDto == null)
            {
                return BadRequest("MenuItem is required");
            }

            await _menuItemService.AddAsync(menuItemDto);

            return Ok("MenuItem Created!");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Cashier")]
        public async Task<IActionResult> UpdateMenuItem(Guid id, [FromBody] MenuItemDto menuItem)
        {
            if (menuItem == null)
            {
                return BadRequest("MenuItem is required");
            }

            await _menuItemService.UpdateAsync(id, menuItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Cashier")]
        public async Task<IActionResult> DeleteMenuItem(Guid id)
        {
            var menuItem = await _menuItemService.GetByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound($"MenuItem with ID {id} not found.");
            }

            await _menuItemService.DeleteAsync(id);
            return NoContent();
        }
    }
}
