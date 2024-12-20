﻿using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Domain.Requests;

namespace RestaurantAPI.Domain.Interfaces.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(Guid id);
        Task<Order> AddAsync(OrderDto orderDto, string userId);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Guid id);
        Task AddItemToOrderAsync(Guid orderId, AddOrderItemRequest request);
        Task RemoveItemFromOrderAsync(Guid orderId, Guid itemId);
    }
}
