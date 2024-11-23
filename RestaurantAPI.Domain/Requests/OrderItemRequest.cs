namespace RestaurantAPI.Domain.Requests
{
    public class OrderItemRequest
    {
        public Guid MenuItemId { get; set; }
        public int Quantity { get; set; }
    }
}
