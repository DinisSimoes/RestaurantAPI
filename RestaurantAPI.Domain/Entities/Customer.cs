using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Domain.Entities
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
