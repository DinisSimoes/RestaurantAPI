using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain.Entities;

namespace RestaurantAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase
    {
        //TODO : Importar logica dps que construir o serviço e interface

        [HttpGet]
        public async Task<IActionResult> GetMenuItems()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenuItem([FromBody] MenuItem menuItem)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuItem menuItem)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            return Ok();
        }
    }
}
