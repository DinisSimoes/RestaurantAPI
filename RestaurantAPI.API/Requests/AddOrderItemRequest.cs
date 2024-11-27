using RestaurantAPI.Domain.DTOs;

namespace RestaurantAPI.API
{
    public class AddOrderItemRequest
    {
        public OrderItemDto orderItem { get; set; }
    }
}
