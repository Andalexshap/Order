namespace Order.Models
{
    public class CartResponse : Response
    {
        public CartResponse()
        {
        }

        public CartResponse(bool success)
        {
            Success = success;
        }

        public CartResponse(Error error)
        {
            Success = false;
            Errors = new List<Error> { error };
        }

        public CartResponse(Cart cart)
        {
            Success = true;
            Cart = cart;
        }

        public CartResponse(Carts cartList)
        {
            Success = true;
            CartList = cartList;
        }

        public Cart? Cart { get; set; }
        public Carts? CartList { get; set; }
    }
}
