using Order.Models;
using Order.Models.Account;

namespace Order.Interfaces
{
    public interface ICartService
    {
        void SetFileName(string filename);
        CartResponse GetAllCarts();
        CartResponse CreateCart(User user, Product product);
        CartResponse UpdateCart(User user, Product product);
        CartResponse DeleteCart(string cartId);
        CartResponse GetCartbyCartId(User user, Product product);
        CartResponse GetCartByUserId(string userId);
        CartResponse GetCartByUserLogin(string login);
    }
}
