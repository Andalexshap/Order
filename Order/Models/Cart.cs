namespace Order.Models
{
    public class Cart
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public List<Product> Products { get; set; }
        public decimal Price { get; set; }
        public int TotalCount { get; set; }
    }
}
