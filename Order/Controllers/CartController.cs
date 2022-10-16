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
            _cartService.SetFileName("cart.json");
            _user = user;
        }

        [HttpGet]
        [Route("allCarts")]
        public ActionResult GetAllCarts()
        {
            if (_user.MemberType != MemberType.Administrator)
            {
                return View("Access denied!");
            }

            var response = _cartService.GetAllCarts();

            return View(response);
        }

        [HttpGet]
        [Route("create")]
        public ActionResult CreateCart(string productId, int quantity)
        {
            var response = _cartService.CreateCart(_user.Key, productId, quantity);

            return View(response);
        }

        [HttpGet]
        [Route("update")]
        public ActionResult UpdateCart(string productId, int quantity)
        {
            var response = _cartService.UpdateCart(_user.Key, productId, quantity);

            return View(response);
        }

        [HttpGet]
        [Route("delete")]
        public ActionResult DeleteCart(string cartId)
        {
            if (_user.MemberType != MemberType.Administrator || !string.IsNullOrEmpty(_user.Key))
            {
                return Redirect("~/account/login");
            }

            var response = _cartService.DeleteCart(cartId);

            return View(response);
        }

        [HttpGet]
        [Route("getCartByCartId")]
        public ActionResult GetCartbyCartId(string cartId)
        {
            if (_user.MemberType != MemberType.Administrator || !string.IsNullOrEmpty(_user.Key))
            {
                return Redirect("~/account/login");
            }

            var response = _cartService.GetCartbyCartId(cartId);

            return View(response);
        }

        [HttpGet]
        [Route("getCartByUserId")]
        public ActionResult GetCartbyUserId(string userId)
        {
            if (_user.MemberType != MemberType.Administrator || !string.IsNullOrEmpty(_user.Key))
            {
                return Redirect("~/account/login");
            }

            var response = _cartService.GetCartByUserId(_user.Key);

            return View(response);
        }
    }
}
