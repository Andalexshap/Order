using Newtonsoft.Json;
using Order.Interfaces;
using Order.Models;

namespace Order.Services
{
    public class CartService : ICartService
    {
        private string FileName;
        private readonly IProductService _productService;
        public CartService(IProductService productService)
        {
            _productService = productService;
        }

        public void SetFileName(string filename)
        {
            FileName = filename;
        }

        public Carts GetAllCarts()
        {
            Carts response;

            try
            {
                response = JsonConvert
                    .DeserializeObject<Carts>(File
                    .ReadAllText(FileName));
            }
            catch
            {
                return null;

            }
            return response;
        }

        public CartResponse CreateCart(string userId, string productId, int? quantity)
        {
            var сarts = GetAllCarts();

            if (сarts == null)
            {
                сarts = new Carts();
                сarts.AllCarts = new List<Cart>();

                using (StreamWriter writer = File.CreateText(FileName))
                {
                    string output = JsonConvert.SerializeObject(сarts);
                    writer.Write(output);
                }
            }

            var cart = сarts.AllCarts.FirstOrDefault(x => x.UserId == userId);

            if (cart != null)
            {
                UpdateCart(userId, productId, quantity);
            }

            var response = _productService.GetProductById(productId);

            if (quantity > response.Product.Inventory)
            {
                quantity = response.Product.Inventory;
            }

            cart = new Cart
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Products = new List<Product> { response.Product },
                Price = response.Product.Price,
                TotalCount = quantity
            };

            using (StreamWriter writer = File.CreateText(FileName))
            {
                string output = JsonConvert.SerializeObject(сarts);
                writer.Write(output);
            }

            _productService.UpdateProduct(response.Product, -quantity);

            return new CartResponse
            {
                Sucsess = true,
                Cart = cart
            };
        }

        public CartResponse UpdateCart(string userId, string productId, int? quantity)
        {
            var сarts = GetAllCarts();

            if (сarts == null)
            {
                CreateCart(userId, productId, quantity);
            }

            var cart = сarts.AllCarts.FirstOrDefault(x => x.UserId == userId);

            if (cart == null)
            {
                CreateCart(userId, productId, quantity);

            }

            var response = _productService.GetProductById(productId);

            if (quantity > response.Product.Inventory)
            {
                quantity = response.Product.Inventory;
            }

            cart.Products.Add(response.Product);
            cart.Price += response.Product.Price;
            cart.TotalCount += quantity;

            using (StreamWriter writer = File.CreateText(FileName))
            {
                string output = JsonConvert.SerializeObject(сarts);
                writer.Write(output);
            }

            _productService.UpdateProduct(response.Product, -quantity);

            return new CartResponse
            {
                Sucsess = true,
                Cart = cart
            };
        }

        public CartResponse DeleteCart(string cartId)
        {
            var сarts = GetAllCarts();

            if (сarts == null)
            {
                return new CartResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "001",
                            Message = $"Cart, with CartId = {cartId}, not found",
                            Target = nameof(DeleteCart)
                        }
                    }
                };
            }

            var cart = сarts.AllCarts.FirstOrDefault(x => x.Id == cartId);

            if (cart == null)
            {
                return new CartResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "001",
                            Message = $"Cart, with CartId = {cartId}, not found",
                            Target = nameof(DeleteCart)
                        }
                    }
                };
            }

            сarts.AllCarts.Remove(cart);

            using (StreamWriter writer = File.CreateText(FileName))
            {
                string output = JsonConvert.SerializeObject(сarts);
                writer.Write(output);
            }

            return new CartResponse
            {
                Sucsess = true
            };
        }

        public CartResponse GetCartbyCartId(string cartId)
        {
            var сarts = GetAllCarts();

            if (сarts == null)
            {
                return new CartResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "001",
                            Message = $"Cart, with CartId = {cartId}, not found",
                            Target = nameof(GetCartbyCartId)
                        }
                    }
                };
            }

            var cart = сarts.AllCarts.FirstOrDefault(x => x.Id == cartId);

            if (cart == null)
            {
                return new CartResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "001",
                            Message = $"Cart, with CartId = {cartId}, not found",
                            Target = nameof(GetCartbyCartId)
                        }
                    }
                };
            }

            return new CartResponse
            {
                Sucsess = true,
                Cart = cart
            };
        }

        public CartResponse GetCartByUserId(string userId)
        {
            var сarts = GetAllCarts();

            if (сarts == null)
            {
                return new CartResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "002",
                            Message = $"Cart, with UserId = {userId}, not found",
                            Target = nameof(GetCartByUserId)
                        }
                    }
                };
            }

            var cart = сarts.AllCarts.FirstOrDefault(x => x.UserId == userId);

            if (cart == null)
            {
                return new CartResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "002",
                            Message = $"Cart, with UserId = {userId}, not found",
                            Target = nameof(GetCartByUserId)
                        }
                    }
                };
            }

            return new CartResponse
            {
                Sucsess = true,
                Cart = cart
            };
        }
    }
}
