using MediatR;
using RestaurantAPI.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.Order.UpdateOrder
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, UpdateOrderResponse>
    {
        private readonly IOrderService _orderService;

        public UpdateOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<UpdateOrderResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetByIdAsync(request.Id);

            if (order == null)
            {
                return new UpdateOrderResponse
                {
                    Success = false,
                    Message = $"Order with ID {request.Id} not found."
                };
            }

            order.Status = request.Status;
            await _orderService.UpdateAsync(order);

            return new UpdateOrderResponse
            {
                Success = true,
                Message = "Order status updated successfully."
            };
        }
    }
}
