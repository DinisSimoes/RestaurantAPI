using MediatR;
using Microsoft.AspNetCore.Http;
using RestaurantAPI.Application.Services;
using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.Order.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
    {
        private readonly IOrderService _orderService;
        private readonly IUserContextService _userContextService;

        public CreateOrderHandler(IOrderService orderService, IUserContextService userContextService)
        {
            _orderService = orderService;
            _userContextService = userContextService;
        }

        public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = _userContextService.GetCurrentUserId();

                var orderDto = new OrderDto
                {
                    customer = request.customer,
                    OrderItems = request.OrderItems,
                };

                var order = await _orderService.AddAsync(orderDto, userId);

                return new CreateOrderResponse
                {
                    Message = "Order created successfully.",
                    Success = true
                };
            }
            catch (ArgumentException ex)
            {
                return new CreateOrderResponse
                {
                    Message = ex.Message,
                    Success = false
                };
            }
            catch (Exception ex)
            {
                return new CreateOrderResponse
                {
                    Message = "An error occurred while creating the order. " + ex.Message,
                    Success = false
                };
            }

        }
    }
}
