using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Application.Commands.MenuItem.CreateMenuItem;
using RestaurantAPI.Application.Commands.MenuItem.DeleteMenuItem;
using RestaurantAPI.Application.Commands.MenuItem.UpdateMenuItem;
using RestaurantAPI.Application.Queries.GetMenuItemById;
using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Interfaces.Services;

namespace RestaurantAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MenuItemController(IMediator mediator)
        {
            _mediator = mediator;
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
            var query = new GetMenuItemByIdQuery { Id = id };
            var response = await _mediator.Send(query);

            if (response.Success)
            {
                return Ok(response.MenuItem);
            }

            return NotFound(new { Message = response.Message });
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
                return BadRequest("MenuItem is required.");
            }

            var command = new CreateMenuItemCommand { MenuItem = menuItemDto };
            var response = await _mediator.Send(command);

            if (response.Success)
            {
                return Ok(new { Message = response.Message });
            }

            return BadRequest(new { Message = response.Message });
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

            var command = new UpdateMenuItemCommand { Id = id, MenuItem = menuItem };
            var response = await _mediator.Send(command);

            if (response.Success)
            {
                return NoContent();
            }

            return NotFound(new { Message = response.Message });
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
            var command = new DeleteMenuItemCommand { Id = id };
            var response = await _mediator.Send(command);

            if (response.Success)
            {
                return NoContent();
            }

            return NotFound(new { Message = response.Message });
        }
    }
}
