using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Domain.DTOs
{
    public class OrderItemDto
    {
        [Required(ErrorMessage = "MenuItem is required.")]
        public string MenuItem { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }
    }
}
