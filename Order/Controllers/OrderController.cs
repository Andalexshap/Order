using Microsoft.AspNetCore.Mvc;
using Order.Interfaces;
using Order.Models;
using Order.Models.Account;

namespace Order.Controllers
{
    [Route("order")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly User _user;

        public OrderController(IOrderService orderService, User user)
        {
            _orderService = orderService;
            _orderService.SetFileName("orders.json");
            _user = user;
        }

        [HttpGet]
        [Route("create")]
        public ActionResult CreateOrder(string cartId)
        {
            if (_user.Key == null)
            {
                return Redirect("~/account/login");
            }

            var response = _orderService.CreateOrder(_user.Key, cartId);

            return View(response);

        }

        [HttpGet]
        [Route("get-order")]
        public ActionResult GetOrder(string OrderId)
        {
            if (_user.Key == null)
            {
                return Redirect("~/account/login");
            }

            var resposne = _orderService.GetOrder(OrderId);

            return View(resposne);
        }

        [HttpGet]
        [Route("get-all-orders")]
        public ActionResult GetAllOrders()
        {
            if (_user.MemberType != MemberType.Administrator)
            {
                return View("Access denied!");
            }

            var response = _orderService.GetListOrders();

            return View(response);
        }

        [HttpGet]
        [Route("get-order-status")]
        public ActionResult GetOrderStatus(string orderId)
        {
            if (_user.Key == null)
            {
                return Redirect("~/account/login");
            }

            var response = _orderService.GetOrderStatus(orderId);

            return View(response);
        }

        [HttpGet]
        [Route("get-user-order")]
        public ActionResult GetUserOrder(string userId)
        {
            if (_user.Key == null)
            {
                return Redirect("~/account/login");
            }
            var response = _orderService.GetUserOrder(userId);

            return View(response);
        }

        [HttpGet]
        [Route("change-status")]
        public ActionResult ChangeStatus(string orderId, OrderStatus status)
        {
            if (_user.MemberType != MemberType.Administrator)
            {
                return View("Access denied!");
            }

            var response = _orderService.UpdateStatus(orderId, status);

            return View(response);
        }

        [HttpGet]
        [Route("delete")]
        public ActionResult DeleteOrder(string orderId)
        {
            if (_user.MemberType != MemberType.Administrator)
            {
                return View("Access denied!");
            }

            var response = _orderService.DeleteOrder(orderId);

            return View(response);
        }
    }
}
