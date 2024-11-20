using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Application.Services;
using RestaurantAPI.Domain.Entities;

namespace RestaurantAPI.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user, [FromQuery] string password)
        {
            var result = await _authService.RegisterUserAsync(user, password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user, [FromQuery] string password)
        {
            var token = await _authService.AuthenticateUserAsync(user.UserName, password);
            if (token == null)
                return Unauthorized();

            return Ok(new { Token = token });
        }
    }
}
