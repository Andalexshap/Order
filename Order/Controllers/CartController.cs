using Microsoft.AspNetCore.Mvc;
using Order.Interfaces;
using Order.Models.Account;

namespace Order.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly User _user;
        public CartController(ICartService cartService, User user)
        {
            _cartService = cartService;
            _cartService.SetFileName("carts.json");
            _user = user;
        }

        [HttpGet]
        [Route("all-carts")]
        public ActionResult GetAllCarts()
        {
            if (_user.MemberType != MemberType.Administrator)
            {
                return View("Access denied!");
            }

            var response = _cartService.GetListCarts();

            return View(response);
        }

        [HttpGet]
        [Route("create")]
        public ActionResult CreateCart(string productId, int quantity)
        {
            var response = _cartService.CreateCart(_user.Id, productId, quantity);

            return View(response);
        }

        [HttpGet]
        [Route("update")]
        public ActionResult UpdateCart(string productId, int quantity)
        {
            var response = _cartService.UpdateCart(_user.Id, productId, quantity);

            return View(response);
        }

        [HttpGet]
        [Route("delete")]
        public ActionResult DeleteCart(string cartId)
        {
            if (string.IsNullOrEmpty(_user.Id))
            {
                return Redirect("~/account/login");
            }

            var response = _cartService.DeleteCart(cartId);

            return View(response);
        }

        [HttpGet]
        [Route("get-cart-by-cart-id")]
        public ActionResult GetCartbyCartId(string cartId)
        {
            if (string.IsNullOrEmpty(_user.Id))
            {
                return Redirect("~/account/login");
            }

            var response = _cartService.GetCartbyCartId(cartId);

            return View(response);
        }

        [HttpGet]
        [Route("get-cart-by-user-id")]
        public ActionResult GetCartbyUserId(string userId)
        {
            if (string.IsNullOrEmpty(_user.Id))
            {
                return Redirect("~/account/login");
            }

            var response = _cartService.GetCartByUserId(userId);

            return View(response);
        }
    }
}
