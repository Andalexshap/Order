using Order.Models.Account;

namespace Order.Models
{
    public class Order
    {
        public string Id { get; set; }
        public User User { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public OrderStatus Status { get; set; }
        public List<Product> Products { get; set; }
    }
}
