using RestaurantAPI.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Queries.GetMenuItemById
{
    public class GetMenuItemByIdResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public MenuItemDto MenuItem { get; set; }
    }
}
