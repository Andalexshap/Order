using Microsoft.AspNetCore.Mvc;
using Order.Interfaces;
using Order.Models;
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
            return View();
        }

        [HttpGet]
        [Route("create")]
        public ActionResult CreateCart(User user, Product product)
        {
            return View();
        }

        [HttpGet]
        [Route("update")]
        public ActionResult UpdateCart(User user, Product product)
        {
            return View();
        }

        [HttpGet]
        [Route("delete")]
        public ActionResult DeleteCart(string cartId)
        {

            return View();
        }


        [HttpGet]
        [Route("find")]
        public ActionResult FindCartbyCartId(User user, Product product)
        {
            return View();
        }
    }
}
