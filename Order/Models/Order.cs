namespace Order.Models
{
    public class Order
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? TotalCount { get; set; }
        public OrderStatus Status { get; set; }
        public List<Product> Products { get; set; }
    }
}
