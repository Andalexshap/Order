namespace Order.Models
{
    public class Orders
    {
        public List<Order> AllOrders { get; set; }

        public void AddOrder(Order order)
        {
            AllOrders.Add(order);
        }
    }
}
