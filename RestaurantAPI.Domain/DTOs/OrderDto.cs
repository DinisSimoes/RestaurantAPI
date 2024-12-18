using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Domain.DTOs
{
    public class OrderDto
    {
        [Required(ErrorMessage = "customer data is required")]
        public required CustomerDto Customer {  get; set; }
        [Required(ErrorMessage = "customer data is required")]
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

    }
}
