namespace Order.Models
{
    public class OrderResponse : Response
    {
        public OrderResponse()
        {
        }

        public OrderResponse(bool success)
        {
            Success = success;
        }

        public OrderResponse(Error error)
        {
            Success = false;
            Errors = new List<Error> { error };
        }

        public OrderResponse(Order order)
        {
            Success = true;
            Order = order;
        }

        public OrderResponse(Orders orderList)
        {
            Success = true;
            OrderList = orderList;
        }

        public Order Order { get; set; }
        public Orders? OrderList { get; set; }
    }
}
