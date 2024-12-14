using MediatR;
using RestaurantAPI.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.MenuItem.CreateMenuItem
{
    public class CreateMenuItemCommand : IRequest<CreateMenuItemResponse>
    {
        public MenuItemDto MenuItem { get; set; }
    }
}
