namespace Order.Models
{
    public class OrderResponse : Response
    {
        public Order Order { get; set; }
        public List<Order>? OrderList { get; set; }
    }
}
