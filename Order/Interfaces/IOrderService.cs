using Order.Models;

namespace Order.Interfaces
{
    public interface IOrderService
    {
        void SetFileName(string filename);
        OrderResponse CreateOrder(string userId, string cartId);
        public OrderResponse GetOrder(string orderId);
        OrderStatus GetOrderStatus(string orderId);
        OrderResponse GetUserOrder(string userId);
        OrderResponse UpdateStatus(string orderId, OrderStatus status);
        OrderResponse DeleteOrder(string orderId);
        OrderResponse GetListOrders();
    }
}
