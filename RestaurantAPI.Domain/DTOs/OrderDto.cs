using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Domain.DTOs
{
    public class OrderDto
    {
        [Required(ErrorMessage = "Customer data is required")]
        public required CustomerDto Customer {  get; set; }
        [Required(ErrorMessage = "OrderItems data is required")]
        public required ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

    }
}
