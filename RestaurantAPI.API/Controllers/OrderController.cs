using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain.Entities;

namespace RestaurantAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            return Ok();
        }
    }
}
