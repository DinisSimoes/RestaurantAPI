using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.OrderItem.DeleteItemFromOrder
{
    public class DeleteItemFromOrderCommand : IRequest<DeleteItemFromOrderResponse>
    {
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
    }
}
