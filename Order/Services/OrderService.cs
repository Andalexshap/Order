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

            if (!response.Sucsess)
            {
                return new OrderResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "001",
                            Message = "Cart not found!",
                            Target = nameof(CreateOrder)
                        }
                    }

                };
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

            return new OrderResponse
            {
                Sucsess = true,
                Order = order
            };
        }
        // ToDo переделать под получение нескольких ордеров одного пользователя
        public OrderResponse GetOrder(string orderId)
        {
            var orders = GetOrders();

            if (orders == null)
            {
                return new OrderResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "002",
                            Message = $"Order, with ID = {orderId}, not found!",
                            Target = nameof(GetOrder)
                        }
                    }

                };
            }

            var userOrders = orders.AllOrders.FindAll(x => x.Id == orderId);

            if (userOrders == null)
            {
                return new OrderResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "002",
                            Message = $"Order, with ID = {orderId}, not found!",
                            Target = nameof(GetOrder)
                        }
                    }

                };
            }

            return new OrderResponse
            {
                Sucsess = true,
                OrderList = userOrders
            };
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
                return new OrderResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "003",
                            Message = $"Order, with UserID = {userId}, not found!",
                            Target = nameof(GetUserOrder)
                        }
                    }

                };
            }

            var order = orders.AllOrders.FirstOrDefault(x => x.UserId == userId);

            if (order == null)
            {
                return new OrderResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "003",
                            Message = $"Order, with UserID = {userId}, not found!",
                            Target = nameof(GetUserOrder)
                        }
                    }

                };
            }

            return new OrderResponse
            {
                Sucsess = true,
                Order = order
            };
        }

        public OrderResponse UpdateStatus(string orderId, OrderStatus status)
        {
            var orders = GetOrders();

            if (orders == null)
            {
                return new OrderResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "002",
                            Message = $"Order, with ID = {orderId}, not found!",
                            Target = nameof(UpdateStatus)
                        }
                    }

                };
            }

            var order = orders.AllOrders.FirstOrDefault(x => x.Id == orderId);

            if (order == null)
            {
                return new OrderResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "002",
                            Message = $"Order, with ID = {orderId}, not found!",
                            Target = nameof(UpdateStatus)
                        }
                    }
                };
            }

            order.Status = status;

            using (StreamWriter writer = File.CreateText(FileName))
            {
                string output = JsonConvert.SerializeObject(orders);
                writer.Write(output);
            }

            return new OrderResponse
            {
                Sucsess = true,
                Order = order
            };
        }

        public OrderResponse DeleteOrder(string orderId)
        {
            var orders = GetOrders();

            if (orders == null)
            {
                return new OrderResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "002",
                            Message = $"Order, with ID = {orderId}, not found!",
                            Target = nameof(DeleteOrder)
                        }
                    }

                };
            }

            var order = orders.AllOrders.FirstOrDefault(x => x.Id == orderId);

            if (order == null)
            {
                return new OrderResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "002",
                            Message = $"Order, with ID = {orderId}, not found!",
                            Target = nameof(DeleteOrder)
                        }
                    }

                };
            }

            orders.AllOrders.Remove(order);

            FileName.WriteData(orders);

            return new OrderResponse
            {
                Sucsess = true,
            };
        }

        public OrderResponse GetListOrders()
        {
            OrderResponse response = new OrderResponse();

            var orders = GetOrders();

            if (orders == null)
            {
                response.Sucsess = false;
                response.Error = new List<Error>
                {
                    new Error
                    {
                        Code = "001",
                        Message = "List carts not found",
                        Target = nameof(GetOrders)
                    }
                };
            }

            response.OrderList = orders;

            return response;
        }
    }
}
