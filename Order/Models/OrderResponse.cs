namespace Order.Models
{
    public class OrderResponse : Response
    {
        public Order Order { get; set; }
        public Orders? OrderList { get; set; }
    }
}
