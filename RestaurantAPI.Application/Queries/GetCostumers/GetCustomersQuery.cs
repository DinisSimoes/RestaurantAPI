using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Queries.GetCustomers
{
    public class GetCustomersQuery : IRequest<GetCustomersResponse>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public GetCustomersQuery(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }
    }
}
