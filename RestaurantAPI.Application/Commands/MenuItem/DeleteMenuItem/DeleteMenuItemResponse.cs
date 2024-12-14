using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.MenuItem.DeleteMenuItem
{
    public class DeleteMenuItemResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
