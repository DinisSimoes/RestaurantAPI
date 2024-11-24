using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Domain.Entities
{
    public class MenuItem
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int PriceCents { get; set; }
    }
}
