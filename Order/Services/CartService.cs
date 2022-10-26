using Order.Interfaces;
using Order.Models;
using Order.Models.Account;
using Order.Utils;

namespace Order.Services
{
    public class CartService : ICartService
    {
        private string FileName = @"carts.json";

        private readonly IProductService _productService;

        public CartService(IProductService productService)
        {
            _productService = productService;

            _productService.SetFileName("products.json");
        }

        public void SetFileName(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                FileName = filename;
            }
        }

        public CartResponse GetListCarts()
        {
            var carts = GetCarts();

            if (carts == null)
            {
                return new CartResponse(new Error
                {
                    Code = "001",
                    Message = "List carts not found",
                    Target = nameof(GetCarts)
                });
            }

            return new CartResponse(carts);
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
                return UpdateCart(userId, productId, quantity);
            }

            var response = _productService.GetProductById(productId);

            if (quantity > response.Product.Inventory)
            {
                response.Product.Quantity = response.Product.Inventory;
            }
            else
            {
                response.Product.Quantity = quantity;
            }

            cart = new Cart
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Products = new List<Product> { response.Product },
                Price = response.Product.Price * response.Product.Quantity,
                TotalCount = response.Product.Quantity
            };

            carts.AddCart(cart);

            FileName.WriteData(carts);

            return new CartResponse(cart);
        }

        public CartResponse UpdateCart(string userId, string productId, int? quantity)
        {
            var carts = GetCarts();

            if (carts == null)
            {
                return CreateCart(userId, productId, quantity);
            }

            var cart = carts.AllCarts.FirstOrDefault(x => x.UserId == userId);

            if (cart == null)
            {
                CreateCart(userId, productId, quantity);
            }

            RecalculateCart(cart!.Id);

            var response = _productService.GetProductById(productId);
            var cartProduct = cart.Products.FirstOrDefault(x => x.Id == response.Product.Id);

            if (response?.Product?.Id == cartProduct?.Id)
            {
                if (quantity + cartProduct.Quantity > response.Product.Inventory)
                {
                    cartProduct.Quantity = response.Product.Inventory;
                }
                else
                {
                    cartProduct.Quantity += quantity;
                }
            }
            else
            {
                if (quantity > response.Product.Inventory)
                {
                    response.Product.Quantity = response.Product.Inventory;
                }
                else
                {
                    response.Product.Quantity = quantity;
                }

                cart.Products.Add(response.Product);
                cart.Price += response.Product.Price * response.Product.Quantity;
                cart.TotalCount += response.Product.Quantity;
            }

            FileName.WriteData(carts);

            return new CartResponse(cart);
        }

        public CartResponse DeleteCart(string cartId)
        {
            var carts = GetCarts();

            if (carts == null)
            {
                return new CartResponse(new Error
                {
                    Code = "001",
                    Message = "List carts not found",
                    Target = nameof(GetCarts)
                });
            }

            var cart = carts.AllCarts.FirstOrDefault(x => x.Id == cartId);

            if (cart == null)
            {
                return new CartResponse(new Error
                {
                    Code = "001",
                    Message = $"Cart, with CartId = {cartId}, not found",
                    Target = nameof(DeleteCart)
                });
            }

            carts.RemoveCart(cart);

            FileName.WriteData(carts);

            return new CartResponse(true);
        }

        public CartResponse GetCartbyCartId(string cartId)
        {
            var carts = GetCarts();

            if (carts == null)
            {
                return new CartResponse(new Error
                {
                    Code = "001",
                    Message = "List carts not found",
                    Target = nameof(GetCarts)
                });
            }

            var cart = carts.AllCarts.FirstOrDefault(x => x.Id == cartId);

            if (cart == null)
            {
                return new CartResponse(new Error
                {
                    Code = "001",
                    Message = $"Cart, with CartId = {cartId}, not found",
                    Target = nameof(GetCartbyCartId)
                });
            }

            return new CartResponse(cart);
        }

        public CartResponse GetCartByUserId(string userId)
        {
            var carts = GetCarts();

            if (carts == null)
            {
                return new CartResponse(new Error
                {
                    Code = "001",
                    Message = "List carts not found",
                    Target = nameof(GetCarts)
                });
            }

            var cart = carts.AllCarts.FirstOrDefault(x => x.UserId == userId);

            if (cart == null)
            {
                return new CartResponse(new Error
                {
                    Code = "002",
                    Message = $"Cart, with UserId = {userId}, not found",
                    Target = nameof(GetCartByUserId)
                });
            }

            return new CartResponse(cart);
        }

        public CartResponse RecalculateCart(string cartId)
        {
            var carts = GetCarts();

            if (carts == null)
            {
                return new CartResponse(new Error
                {
                    Code = "001",
                    Message = "List carts not found",
                    Target = nameof(GetCarts)
                });
            }

            var cart = carts.AllCarts.FirstOrDefault(x => x.Id == cartId);

            if (cart == null)
            {
                return new CartResponse(new Error
                {
                    Code = "001",
                    Message = $"Cart, with CartId = {cartId}, not found",
                    Target = nameof(GetCartbyCartId)
                });
            }

            cart.Price = 0;
            cart.TotalCount = 0;

            foreach (Product product in cart.Products)
            {
                var result = _productService.GetProductById(product.Id);

                if (result == null) cart.Products.Remove(product);

                if (result!.Product.Price != product.Price)
                {
                    product.Price = result.Product.Price;
                }

                if (result.Product.Inventory < product.Quantity)
                {
                    product.Quantity = result.Product.Inventory;
                }

                cart.Price += product.Price * product.Quantity;
                cart.TotalCount += product.Quantity;
            }

            FileName.WriteData(carts);

            return new CartResponse(cart);
        }

        private Carts? GetCarts()
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
    }
}
