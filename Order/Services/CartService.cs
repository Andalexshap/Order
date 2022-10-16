using Order.Interfaces;
using Order.Models;
using Order.Models.Account;

namespace Order.Services
{
    public class CartService : ICartService
    {
        private string FileName;

        public void SetFileName(string filename)
        {
            FileName = filename;
        }

        public CartResponse GetAllCarts()
        {
            throw new NotImplementedException();
        }

        public CartResponse CreateCart(User user, Product product)
        {
            throw new NotImplementedException();
        }

        public CartResponse UpdateCart(User user, Product product)
        {
            throw new NotImplementedException();
        }

        public CartResponse DeleteCart(string cartId)
        {
            throw new NotImplementedException();
        }

        public CartResponse GetCartbyCartId(User user, Product product)
        {
            throw new NotImplementedException();
        }

        public CartResponse GetCartByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public CartResponse GetCartByUserLogin(string login)
        {
            throw new NotImplementedException();
        }
    }
}
