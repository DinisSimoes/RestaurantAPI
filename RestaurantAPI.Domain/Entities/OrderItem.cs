using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Domain.Entities
{
    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid ItemId { get; set; }
        public MenuItem MenuItem { get; set; }

        public int Quantity { get; set; }
    }
}
