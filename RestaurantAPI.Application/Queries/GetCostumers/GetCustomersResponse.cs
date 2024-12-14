using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Queries.GetCustomers
{
    public class GetCustomersResponse
    {
        public PageResult<Customer> Customers { get; set; }
    }
}
