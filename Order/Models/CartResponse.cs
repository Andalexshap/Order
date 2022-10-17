namespace Order.Models
{
    public class CartResponse : Response
    {
        public Cart? Cart { get; set; }
        public Carts? CartList { get; set; }
    }
}
