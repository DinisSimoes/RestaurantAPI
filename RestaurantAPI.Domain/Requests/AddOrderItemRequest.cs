using System.ComponentModel.DataAnnotations;
using RestaurantAPI.Domain.DTOs;

namespace RestaurantAPI.Domain.Requests
{
    public class AddOrderItemRequest
    {
        [Required(ErrorMessage = "orderItem is required.")]
        public OrderItemDto orderItem { get; set; }
    }
}
