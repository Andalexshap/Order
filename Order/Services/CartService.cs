using Newtonsoft.Json;
using Order.Interfaces;
using Order.Models;
using Order.Models.Account;
using Order.Utils;

namespace Order.Services
{
    public class CartService : ICartService
    {
        private readonly IProductService _productService;
        private string FileName = @"carts.json";

        public CartService(IProductService productService)
        {
            _productService = productService;
        }

        public void SetFileName(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                FileName = filename;
            }
        }

        public Carts? GetCarts()
        {
            Carts? response;

            try
            {
                response = FileName.GetData<Carts>();
            }
            catch
            {
                return null;

            }

            return response;
        }

        public CartResponse CreateCart(string userId, string productId, int? quantity)
        {
            var carts = GetCarts();

            if (carts == null)
            {
                carts = new Carts();
                carts.AllCarts = new List<Cart>();
            }

            var cart = carts.AllCarts.FirstOrDefault(x => x.UserId == userId);
            
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

            FileName.WriteData(carts);

            _productService.UpdateProduct(response.Product, -quantity);

            return new CartResponse
            {
                Sucsess = true,
                Cart = cart
            };
        }

        public CartResponse UpdateCart(string userId, string productId, int? quantity)
        {
            var carts = GetCarts();

            if (carts == null)
            {
                CreateCart(userId, productId, quantity);
            }

            var cart = carts.AllCarts.FirstOrDefault(x => x.UserId == userId);

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

            FileName.WriteData(carts);

            _productService.UpdateProduct(response.Product, -quantity);

            return new CartResponse
            {
                Sucsess = true,
                Cart = cart
            };
        }

        public CartResponse DeleteCart(string cartId)
        {
            var carts = GetCarts();

            if (carts == null)
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

            var cart = carts.AllCarts.FirstOrDefault(x => x.Id == cartId);

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

            carts.AllCarts.Remove(cart);

            FileName.WriteData(carts);

            return new CartResponse
            {
                Sucsess = true
            };
        }

        public CartResponse GetCartbyCartId(string cartId)
        {
            var carts = GetCarts();

            if (carts == null)
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

            var cart = carts.AllCarts.FirstOrDefault(x => x.Id == cartId);

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
            var carts = GetCarts();

            if (carts == null)
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

            var cart = carts.AllCarts.FirstOrDefault(x => x.UserId == userId);

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
