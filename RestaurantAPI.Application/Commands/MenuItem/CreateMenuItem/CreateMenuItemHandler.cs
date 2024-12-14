using MediatR;
using RestaurantAPI.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.MenuItem.CreateMenuItem
{
    internal class CreateMenuItemHandler : IRequestHandler<CreateMenuItemCommand, CreateMenuItemResponse>
    {
        private readonly IMenuItemService _menuItemService;

        public CreateMenuItemHandler(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        public async Task<CreateMenuItemResponse> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
        {
            // Verifique se o menuItem é válido
            if (request.MenuItem == null)
            {
                return new CreateMenuItemResponse
                {
                    Success = false,
                    Message = "MenuItem data is required."
                };
            }

            await _menuItemService.AddAsync(request.MenuItem);

            return new CreateMenuItemResponse
            {
                Success = true,
                Message = "MenuItem created successfully."
            };
        }
    }
}
