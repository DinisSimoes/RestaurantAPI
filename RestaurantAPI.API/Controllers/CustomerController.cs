using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain.Entities;

namespace RestaurantAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCustomers([FromQuery] string phoneNumber)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            return Ok();
        }
    }
}
