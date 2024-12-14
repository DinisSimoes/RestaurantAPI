using MediatR;
using RestaurantAPI.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Queries.GetCustomers
{
    public class GetCustomersHandler : IRequestHandler<GetCustomersQuery, GetCustomersResponse>
    {
        private readonly ICustomerService _customerService;

        public GetCustomersHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<GetCustomersResponse> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerService.GetCustomersAsync(request.Page, request.PageSize);

            return new GetCustomersResponse
            {
                Customers = customers
            };
        }
    }
}
