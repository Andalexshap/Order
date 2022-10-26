using Newtonsoft.Json;
using Order.Interfaces;
using Order.Models;
using Order.Utils;

namespace Order.Services
{
    public class OrderService : IOrderService
    {
        private string FileName = @"orders.json";
        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public OrderService(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        public void SetFileName(string filename)
        {
            FileName = filename;
        }

        private Orders? GetOrders()
        {
            Orders? response;

            try
            {
                response = FileName.GetData<Orders>();
            }
            catch
            {
                return null;

            }
            return response;
        }

        public OrderResponse CreateOrder(string userId, string cartId)
        {
            var orders = GetOrders();

            if (orders == null)
            {
                orders = new Orders();
                orders.AllOrders = new List<Models.Order>();
            }

            var response = _cartService.RecalculateCart(cartId);

            if (!response.Success)
            {
                return new OrderResponse(new Error
                {
                    Code = "002",
                    Message = "Cart not found!",
                    Target = nameof(CreateOrder)
                });
            }

            var order = new Models.Order();

            order.Id = Guid.NewGuid().ToString();
            order.Status = OrderStatus.Issued;
            order.TotalPrice = response?.Cart?.Price;
            order.TotalCount = response?.Cart?.TotalCount;
            order.UserId = userId;
            order.Products = response.Cart.Products;

            orders.AddOrder(order);

            FileName.WriteData(orders);

            foreach (Product product in response.Cart.Products)
            {
                _productService.UpdateProduct(product, -product.Quantity);
            }

            _cartService.DeleteCart(cartId);

            return new OrderResponse(order);
        }

        public OrderResponse GetOrder(string orderId)
        {
            var orders = GetOrders();

            if (orders == null)
            {
                return new OrderResponse(new Error
                {
                    Code = "001",
                    Message = $"Orders not found!",
                    Target = nameof(GetOrder)
                });
            }

            var order = orders.AllOrders.FirstOrDefault(x => x.Id == orderId);

            if (order == null)
            {
                return new OrderResponse(new Error
                {
                    Code = "001",
                    Message = $"Orders not found!",
                    Target = nameof(GetOrder)
                });
            }

            return new OrderResponse(order);
        }


        public OrderStatus GetOrderStatus(string orderId)
        {
            var orders = GetOrders();
            if (orders == null)
            {
                return OrderStatus.Error;
            }

            var order = orders.AllOrders.FirstOrDefault(x => x.Id == orderId);

            if (order == null)
            {
                return OrderStatus.Error;
            }

            return order.Status;
        }

        public OrderResponse GetUserOrder(string userId)
        {
            var orders = GetOrders();

            if (orders == null)
            {
                return new OrderResponse(new Error
                {
                    Code = "001",
                    Message = $"Orders not found!",
                    Target = nameof(GetOrder)
                });
            }

            var userOrders = new Orders();
            userOrders.AllOrders = orders.AllOrders.Where(x => x.UserId == userId).ToList();

            if (userOrders.AllOrders == null)
            {
                return new OrderResponse(new Error
                {
                    Code = "003",
                    Message = $"Order, with UserID = {userId}, not found!",
                    Target = nameof(GetUserOrder)
                });
            }

            return new OrderResponse(userOrders);
        }

        public OrderResponse UpdateStatus(string orderId, OrderStatus status)
        {
            var orders = GetOrders();

            if (orders == null)
            {
                return new OrderResponse(new Error
                {
                    Code = "001",
                    Message = $"Orders not found!",
                    Target = nameof(GetOrder)
                });
            }

            var order = orders.AllOrders.FirstOrDefault(x => x.Id == orderId);

            if (order == null)
            {
                return new OrderResponse(new Error
                {
                    Code = "002",
                    Message = $"Order, with ID = {orderId}, not found!",
                    Target = nameof(DeleteOrder)
                });
            }

            order.Status = status;

            using (StreamWriter writer = File.CreateText(FileName))
            {
                string output = JsonConvert.SerializeObject(orders);
                writer.Write(output);
            }

            return new OrderResponse(order);
        }

        public OrderResponse DeleteOrder(string orderId)
        {
            var orders = GetOrders();

            if (orders == null)
            {
                return new OrderResponse(new Error
                {
                    Code = "001",
                    Message = $"Orders not found!",
                    Target = nameof(GetOrder)
                });
            }

            var order = orders.AllOrders.FirstOrDefault(x => x.Id == orderId);

            if (order == null)
            {
                return new OrderResponse(new Error
                {
                    Code = "002",
                    Message = $"Order, with ID = {orderId}, not found!",
                    Target = nameof(DeleteOrder)
                });
            }

            orders.AllOrders.Remove(order);

            FileName.WriteData(orders);

            return new OrderResponse(true);
        }

        public OrderResponse GetListOrders()
        {
            var orders = GetOrders();

            if (orders == null)
            {
                return new OrderResponse(new Error
                {
                    Code = "001",
                    Message = $"Orders not found!",
                    Target = nameof(GetOrder)
                });
            }

            return new OrderResponse(orders);
        }
    }
}
