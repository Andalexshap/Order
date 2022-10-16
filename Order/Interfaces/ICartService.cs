using Order.Models;

namespace Order.Interfaces
{
    public interface ICartService
    {
        void SetFileName(string filename);
        Carts GetAllCarts();
        CartResponse CreateCart(string userId, Product product);
        CartResponse UpdateCart(string userId, Product product);
        CartResponse DeleteCart(string cartId);
        CartResponse GetCartbyCartId(string cartId);
        CartResponse GetCartByUserId(string userId);
    }
}
