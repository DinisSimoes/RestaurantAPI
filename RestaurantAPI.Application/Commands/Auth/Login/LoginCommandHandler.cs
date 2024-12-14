using MediatR;
using RestaurantAPI.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.Auth.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var token = await _authService.AuthenticateUserAsync(request.UserName, request.Password);

            if (token == null)
            {
                return new LoginResponse
                {
                    ErrorMessage = "Invalid username or password."
                };
            }

            return new LoginResponse
            {
                Token = token
            };
        }
    }
}
