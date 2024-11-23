using RestaurantAPI.Domain.DTOs;

namespace RestaurantAPI.Domain.Requests
{
    public class AddOrderItemRequest
    {
        public OrderItemDto orderItem { get; set; }
    }
}
