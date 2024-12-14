using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.MenuItem.DeleteMenuItem
{
    public class DeleteMenuItemCommand : IRequest<DeleteMenuItemResponse>
    {
        public Guid Id { get; set; }
    }
}
