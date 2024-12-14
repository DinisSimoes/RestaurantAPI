using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Domain.Interfaces.Services
{
    public interface IUserContextService
    {
        string GetCurrentUserId();
    }
}
