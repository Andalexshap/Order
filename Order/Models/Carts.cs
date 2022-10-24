namespace Order.Models
{
    public class Carts
    {
        public List<Cart> AllCarts { get; set; }

        public void AddCart(Cart cart)
        {
            AllCarts.Add(cart);
        }

        public void RemoveCart(Cart cart)
        {
            AllCarts.Remove(cart);  
        }
    }
}
