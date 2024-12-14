using MediatR;
using RestaurantAPI.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Queries.GetMenuItemById
{
    public class GetMenuItemByIdHandler : IRequestHandler<GetMenuItemByIdQuery, GetMenuItemByIdResponse>
    {
        private readonly IMenuItemService _menuItemService;

        public GetMenuItemByIdHandler(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        public async Task<GetMenuItemByIdResponse> Handle(GetMenuItemByIdQuery request, CancellationToken cancellationToken)
        {
            var menuItem = await _menuItemService.GetByIdAsync(request.Id);

            if (menuItem == null)
            {
                return new GetMenuItemByIdResponse
                {
                    Success = false,
                    Message = $"MenuItem with ID {request.Id} not found."
                };
            }

            return new GetMenuItemByIdResponse
            {
                Success = true,
                Message = "Menu item retrieved successfully.",
                MenuItem = menuItem
            };
        }
    }
}
