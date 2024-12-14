using MediatR;
using RestaurantAPI.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.OrderItem.AddItemToOrder
{
    public class AddItemToOrderCommand : IRequest<AddItemToOrderResponse>
    {
        public Guid OrderId { get; set; }
        public OrderItemDto OrderItem { get; set; }
    }
}
