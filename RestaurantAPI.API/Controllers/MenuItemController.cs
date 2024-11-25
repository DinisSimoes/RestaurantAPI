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

        /// <summary>
        /// Obtém um item de menu pelo ID.
        /// </summary>
        /// <param name="id">ID do item de menu a ser buscado.</param>
        /// <returns>O item de menu encontrado ou uma mensagem de erro.</returns>
        /// <response code="200">Item de menu encontrado com sucesso.</response>
        /// <response code="404">Item de menu não encontrado.</response>
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

        /// <summary>
        /// Cria um novo item de menu.
        /// </summary>
        /// <param name="menuItemDto">Dados do item de menu a ser criado.</param>
        /// <returns>Mensagem de sucesso.</returns>
        /// <response code="200">Item de menu criado com sucesso.</response>
        /// <response code="400">Dados do item de menu são inválidos ou faltando.</response>
        [HttpPost]
        [Authorize(Roles = "Cashier, Cook")]
        public async Task<IActionResult> CreateMenuItem([FromBody] MenuItemDto menuItemDto)
        {
            if (menuItemDto == null)
            {
                return BadRequest("MenuItem is required");
            }

            await _menuItemService.AddAsync(menuItemDto);

            return Ok("MenuItem Created!");
        }

        /// <summary>
        /// Atualiza um item de menu existente.
        /// </summary>
        /// <param name="id">ID do item de menu a ser atualizado.</param>
        /// <param name="menuItem">Dados atualizados do item de menu.</param>
        /// <returns>Resposta de sucesso ou erro.</returns>
        /// <response code="204">Item de menu atualizado com sucesso.</response>
        /// <response code="400">Dados do item de menu são inválidos.</response>
        /// <response code="404">Item de menu não encontrado.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Cashier, Cook")]
        public async Task<IActionResult> UpdateMenuItem(Guid id, [FromBody] MenuItemDto menuItem)
        {
            if (menuItem == null)
            {
                return BadRequest("MenuItem is required");
            }

            await _menuItemService.UpdateAsync(id, menuItem);
            return NoContent();
        }

        /// <summary>
        /// Deleta um item de menu pelo ID.
        /// </summary>
        /// <param name="id">ID do item de menu a ser deletado.</param>
        /// <returns>Resposta de sucesso ou erro.</returns>
        /// <response code="204">Item de menu deletado com sucesso.</response>
        /// <response code="404">Item de menu não encontrado.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Cashier, Cook")]
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
