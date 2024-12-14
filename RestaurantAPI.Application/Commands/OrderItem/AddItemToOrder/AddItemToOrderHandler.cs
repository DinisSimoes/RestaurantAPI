using MediatR;
using RestaurantAPI.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.OrderItem.AddItemToOrder
{
    public class AddItemToOrderHandler : IRequestHandler<AddItemToOrderCommand, AddItemToOrderResponse>
    {
        private readonly IOrderService _orderService;

        public AddItemToOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<AddItemToOrderResponse> Handle(AddItemToOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _orderService.AddItemToOrderAsync(request.OrderId, request.OrderItem);

                return new AddItemToOrderResponse
                {
                    Success = true,
                    Message = "Item added to the order successfully."
                };
            }
            catch (KeyNotFoundException ex)
            {
                return new AddItemToOrderResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new AddItemToOrderResponse
                {
                    Success = false,
                    Message = $"An internal error occurred: {ex.Message}"
                };
            }
        }
    }
}
