namespace RestaurantAPI.Domain.DTOs
{
    public class OrderDto
    {
        public CustomerDto customer {  get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

    }
}
