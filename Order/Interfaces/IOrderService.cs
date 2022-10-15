using Order.Models;
using Order.Models.Account;

namespace Order.Interfaces
{
    public interface IOrderService
    {
        void SetFileName(string filename);
        OrderResponse CreateOrder(User user, Product request);
        public OrderResponse GetOrder(string orderId);
        Orders GetAllOrders();
        OrderStatus GetOrderStatus(string orderId);
        OrderResponse GetUserOrder(string userId);
        OrderResponse UpdateStatus(string orderId, OrderStatus status);
        OrderResponse DeleteOrder(string orderId);
    }
}
