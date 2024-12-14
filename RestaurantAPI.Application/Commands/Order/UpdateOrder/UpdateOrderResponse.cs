using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.Order.UpdateOrder
{
    public class UpdateOrderResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
