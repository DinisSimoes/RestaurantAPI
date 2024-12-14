using MediatR;
using RestaurantAPI.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.MenuItem.DeleteMenuItem
{
    public class DeleteMenuItemHandler : IRequestHandler<DeleteMenuItemCommand, DeleteMenuItemResponse>
    {
        private readonly IMenuItemService _menuItemService;

        public DeleteMenuItemHandler(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        public async Task<DeleteMenuItemResponse> Handle(DeleteMenuItemCommand request, CancellationToken cancellationToken)
        {
            // Verifica se o item de menu existe
            var menuItem = await _menuItemService.GetByIdAsync(request.Id);
            if (menuItem == null)
            {
                return new DeleteMenuItemResponse
                {
                    Success = false,
                    Message = $"MenuItem with ID {request.Id} not found."
                };
            }

            // Deleta o item de menu
            await _menuItemService.DeleteAsync(request.Id);

            return new DeleteMenuItemResponse
            {
                Success = true,
                Message = "MenuItem deleted successfully."
            };
        }
    }
}
