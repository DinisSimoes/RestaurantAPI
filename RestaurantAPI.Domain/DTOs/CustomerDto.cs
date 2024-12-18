using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Domain.DTOs
{
    public class CustomerDto
    {
        [Required(ErrorMessage = "First name is required.")]
        public required string firstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public required string lastName { get; set; }
        
        [Required(ErrorMessage = "Phone number is required.")]
        public required string phoneNumber { get; set; }
    }
}
