using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain.Entities;

namespace RestaurantAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok();
        }
    }
}
