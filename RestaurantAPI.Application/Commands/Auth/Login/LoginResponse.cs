using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.Auth.Login
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string ErrorMessage { get; set; }
    }
}
