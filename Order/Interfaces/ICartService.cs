using Order.Models;

namespace Order.Interfaces
{
    public interface ICartService
    {
        void SetFileName(string filename);
        Carts? GetCarts();
        CartResponse CreateCart(string userId, string productId, int? quantity);
        CartResponse UpdateCart(string userId, string productId, int? quantity);
        CartResponse DeleteCart(string cartId);
        CartResponse GetCartbyCartId(string cartId);
        CartResponse GetCartByUserId(string userId);
    }
}
