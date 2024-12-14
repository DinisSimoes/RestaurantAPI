using MediatR;
using RestaurantAPI.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.OrderItem.DeleteItemFromOrder
{
    public class DeleteItemFromOrderHandler : IRequestHandler<DeleteItemFromOrderCommand, DeleteItemFromOrderResponse>
    {
        private readonly IOrderService _orderService;

        public DeleteItemFromOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<DeleteItemFromOrderResponse> Handle(DeleteItemFromOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _orderService.RemoveItemFromOrderAsync(request.OrderId, request.ItemId);
                return new DeleteItemFromOrderResponse
                {
                    Success = true,
                    Message = "Item removed from the order successfully."
                };
            }
            catch (KeyNotFoundException ex)
            {
                return new DeleteItemFromOrderResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new DeleteItemFromOrderResponse
                {
                    Success = false,
                    Message = $"An internal error occurred: {ex.Message}"
                };
            }
        }
    }
}
