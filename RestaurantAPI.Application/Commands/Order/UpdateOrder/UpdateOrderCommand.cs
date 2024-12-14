using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.Order.UpdateOrder
{
    public class UpdateOrderCommand : IRequest<UpdateOrderResponse>
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }
}
