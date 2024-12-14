using RestaurantAPI.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.Customer.AddCustomer
{
    public class AddCustomerResponse
    {
        public string Message { get; set; }
        public CustomerDto Customer { get; set; }
    }
}
