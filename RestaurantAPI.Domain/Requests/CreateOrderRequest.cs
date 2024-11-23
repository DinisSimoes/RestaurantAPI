namespace RestaurantAPI.Domain.Requests
{
    public class CreateOrderRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public List<OrderItemRequest> Items { get; set; }
    }
}
