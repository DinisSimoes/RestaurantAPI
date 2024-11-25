using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Application.Services;
using RestaurantAPI.Domain.DTOs;
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

        /// <summary>
        /// Registra um novo usuário. (A função de "convidar")
        /// </summary>
        /// <param name="registerDto">Informações do usuário para registro.</param>
        /// /// <response code="200">Usuário registrado com sucesso.</response>
        /// <response code="400">Erro de validação ou falha no registro.</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterUserAsync(registerDto);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("User registered successfully.");
        }

        /// <summary>
        /// Realiza o login do usuário.
        /// </summary>
        /// <param name="loginDto">Credenciais do usuário (nome de usuário e senha).</param>
        /// <returns>Token JWT ou mensagem de erro.</returns>
        /// <response code="200">Login bem-sucedido, retorna o token JWT.</response>
        /// <response code="400">Erro de validação.</response>
        /// <response code="401">Credenciais inválidas.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.AuthenticateUserAsync(loginDto.UserName, loginDto.Password);

            if (token == null)
                return Unauthorized(new { Message = "Invalid username or password." });

            return Ok(new { Token = token });
        }
    }
}
