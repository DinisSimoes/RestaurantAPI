using MediatR;
using RestaurantAPI.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.MenuItem.UpdateMenuItem
{
    public class UpdateMenuItemCommand : IRequest<UpdateMenuItemResponse>
    {
        public Guid Id { get; set; }
        public MenuItemDto MenuItem { get; set; }
    }
}
