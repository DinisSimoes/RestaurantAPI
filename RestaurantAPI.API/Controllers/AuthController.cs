using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Application.Commands.Auth.Login;
using RestaurantAPI.Application.Commands.Auth.RegisterUser;
using RestaurantAPI.Application.Services;
using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Entities;

namespace RestaurantAPI.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
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
            {
                return BadRequest(ModelState);
            }

            // Envia o comando para o MediatR
            var command = new RegisterUserCommand(
                registerDto.UserName,
                registerDto.Email,
                registerDto.Password,
                registerDto.FirstName,
                registerDto.LastName,
                registerDto.PhoneNumber,
                registerDto.Role
            );

            var response = await _mediator.Send(command);

            if (!response.Succeeded)
            {
                return BadRequest(new { Errors = response.Errors });
            }

            return Ok(new { Message = response.Message });
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

            // Envia o comando para o Mediator
            var command = new LoginCommand
            {
                UserName = loginDto.UserName,
                Password = loginDto.Password
            };

            var response = await _mediator.Send(command);

            if (response.ErrorMessage != null)
            {
                return Unauthorized(new { Message = response.ErrorMessage });
            }

            return Ok(new { Token = response.Token });
        }
    }
}
