﻿using Newtonsoft.Json;
using Order.Interfaces;
using Order.Models;
using Order.Models.Account;

namespace Order.Services
{
    public class OrderService : IOrderService
    {
        private string FileName;
        private readonly IProductService _productService;

        public OrderService(IProductService productService)
        {
            _productService = productService;
        }

        public void SetFileName(string filename)
        {
            FileName = filename;
        }

        public OrderResponse CreateOrder(User user, Product request)
        {
            var orders = GetAllOrders();

            if (orders == null)
            {
                orders = new Orders();
                orders.AllOrders = new List<Models.Order>();

                using (StreamWriter writer = File.CreateText(FileName))
                {
                    string output = JsonConvert.SerializeObject(orders);
                    writer.Write(output);
                }
            }
            var response = _productService.GetProductById(request.Id);

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
                            Message = "Product not found!",
                            Target = nameof(CreateOrder)
                        }
                    }

                };
            }

            var order = new Models.Order();

            order.Id = Guid.NewGuid().ToString();
            order.Status = OrderStatus.Issued;
            order.Quantity++;
            order.TotalPrice += request.Price;
            order.User = user;
            order.Products.Add(request);

            orders = new Orders();
            orders.AllOrders = new List<Models.Order>();

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

        public OrderResponse GetOrder(string orderId)
        {
            var orders = GetAllOrders();

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
                            Target = nameof(GetOrder)
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

        public Orders GetAllOrders()
        {
            Orders response;

            try
            {
                response = JsonConvert
                    .DeserializeObject<Orders>(File
                    .ReadAllText(FileName));
            }
            catch
            {
                return null;

            }
            return response;
        }

        public OrderStatus GetOrderStatus(string orderId)
        {
            var orders = GetAllOrders();
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
            var orders = GetAllOrders();

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

            var order = orders.AllOrders.FirstOrDefault(x => x.User.Key == userId);

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
            var orders = GetAllOrders();

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
            var orders = GetAllOrders();

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

            using (StreamWriter writer = File.CreateText(FileName))
            {
                string output = JsonConvert.SerializeObject(orders);
                writer.Write(output);
            }


            return new OrderResponse
            {
                Sucsess = true,
            };
        }
    }
}