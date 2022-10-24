using Order.Interfaces;
using Order.Models;
using Order.Utils;

namespace Order.Services
{
    public class ProductService : IProductService
    {
        private string FileName = @"products.json";

        public void SetFileName(string filename)
        {
            FileName = filename;
        }

        public ProductResponse GetAllProducts()
        {
            var products = GetProducts();

            if (products == null)
            {
                return new ProductResponse(new Error
                {
                    Code = "002",
                    Message = "File products.json not found!",
                    Target = nameof(UpdateProduct)
                });
            }

            return new ProductResponse(products);
        }

        public ProductResponse AddNewProduct(Product request)
        {
            var products = GetProducts();

            if (products != null)
            {
                var found = products
                .AllProducts
                .FirstOrDefault(x => x.Name == request.Name & x.Description == request.Description);

                if (found != null)
                {
                    return new ProductResponse(new Error
                    {
                        Code = "001",
                        Message = "Product exist!",
                        Target = nameof(AddNewProduct)
                    });
                }
            }
            else
            {
                products = new Products();
                products.AllProducts = new List<Product>();
            }

            var product = request;
            product.Id = Guid.NewGuid().ToString();
            products.AddProduct(product);

            FileName.WriteData(products);

            return new ProductResponse(product);
        }

        public ProductResponse UpdateProduct(Product request, int? quantity)
        {
            var products = GetProducts();

            if (products == null)
            {
                return new ProductResponse(new Error
                {
                    Code = "002",
                    Message = "File products.json not found!",
                    Target = nameof(UpdateProduct)
                });
            }

            var product = products.AllProducts.FirstOrDefault(x => x.Id == request.Id);

            if (product == null)
            {
                return new ProductResponse(new Error
                {
                    Code = "004",
                    Message = $"Product, with ID = {request.Id}, not found!",
                    Target = nameof(DeleteProduct)
                });

            }

            product.Merge(request, quantity);

            FileName.WriteData(products);

            return new ProductResponse(product);
        }

        public ProductResponse DeleteProduct(string id)
        {
            var products = GetProducts();
            if (products == null)
            {
                return new ProductResponse(new Error
                {
                    Code = "002",
                    Message = "File products.json not found!",
                    Target = nameof(UpdateProduct)
                });
            }

            var product = products.AllProducts.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return new ProductResponse(new Error
                {
                    Code = "004",
                    Message = $"Product, with ID = {id}, not found!",
                    Target = nameof(DeleteProduct)
                });
            }

            products.RemoveProduct(product);

            FileName.WriteData(products);

            return new ProductResponse(true);
        }

        public ProductResponse GetProductById(string id)
        {
            var products = GetProducts();

            if (products == null)
            {
                return new ProductResponse(new Error
                {
                    Code = "002",
                    Message = "File products.json not found!",
                    Target = nameof(UpdateProduct)
                });
            }

            var product = products.AllProducts.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return new ProductResponse(new Error
                {
                    Code = "004",
                    Message = $"Product, with ID = {id}, not found!",
                    Target = nameof(DeleteProduct)
                });
            }

            return new ProductResponse(product);
        }

        public Products? GetProducts()
        {
            Products? response;

            try
            {
                response = FileName.GetData<Products>();
            }
            catch
            {
                return null;

            }

            return response;
        }
    }
}
