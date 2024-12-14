using RestaurantAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.Order.CreateOrder
{
    public class CreateOrderResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

    }
}
