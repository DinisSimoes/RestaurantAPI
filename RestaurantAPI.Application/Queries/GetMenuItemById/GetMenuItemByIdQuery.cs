using MediatR;
using RestaurantAPI.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Queries.GetMenuItemById
{
    public class GetMenuItemByIdQuery : IRequest<GetMenuItemByIdResponse>
    {
        public Guid Id { get; set; }
    }
}
