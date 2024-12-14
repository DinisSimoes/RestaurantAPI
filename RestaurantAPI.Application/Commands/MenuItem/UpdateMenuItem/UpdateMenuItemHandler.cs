using MediatR;
using RestaurantAPI.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.MenuItem.UpdateMenuItem
{
    public class UpdateMenuItemHandler : IRequestHandler<UpdateMenuItemCommand, UpdateMenuItemResponse>
    {
        private readonly IMenuItemService _menuItemService;

        public UpdateMenuItemHandler(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        public async Task<UpdateMenuItemResponse> Handle(UpdateMenuItemCommand request, CancellationToken cancellationToken)
        {
            // Validação: Verificar se o menu item existe
            var menuItem = await _menuItemService.GetByIdAsync(request.Id);
            if (menuItem == null)
            {
                return new UpdateMenuItemResponse
                {
                    Success = false,
                    Message = $"MenuItem with ID {request.Id} not found."
                };
            }

            // Atualizar o item de menu
            await _menuItemService.UpdateAsync(request.Id, request.MenuItem);

            return new UpdateMenuItemResponse
            {
                Success = true,
                Message = "MenuItem updated successfully."
            };
        }
    }
}
