using Newtonsoft.Json;
using Order.Interfaces;
using Order.Models;

namespace Order.Servises
{
    public class ProductService : IProductService
    {

        private string FileName;

        public void SetFileName(string filename)
        {
            FileName = filename;
        }

        public Products GetAllProducts()
        {
            Products response;

            try
            {
                response = JsonConvert
                    .DeserializeObject<Products>(File
                    .ReadAllText(FileName));
            }
            catch
            {
                return null;

            }
            return response;
        }

        public ProductResponse AddNewProduct(Product request)
        {
            var products = GetAllProducts();

            if (products != null)
            {
                var found = products
                .AllProducts
                .FirstOrDefault(x => x.Name == request.Name & x.Description == request.Description);

                if (found != null)
                {
                    return new ProductResponse
                    {
                        Sucsess = false,
                        Error = new List<Error>
                        {
                            new Error
                            {
                                Code = "001",
                                Message = "Product exist!",
                                Target = nameof(AddNewProduct)
                            }
                        }

                    };
                }
            }
            else
            {
                products = new Products();
                products.AllProducts = new List<Product>();
            }

            var product = request;
            product.Id = Guid.NewGuid().ToString();
            products.AllProducts.Add(product);

            using (StreamWriter writer = File.CreateText(FileName))
            {
                string output = JsonConvert.SerializeObject(products);
                writer.Write(output);
            }

            return new ProductResponse
            {
                Sucsess = true,
                Product = product
            };
        }

        public ProductResponse UpdateProduct(Product request)
        {
            var products = GetAllProducts();

            if (products == null)
            {
                return new ProductResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "002",
                            Message = "File products.json not found!",
                            Target = nameof(UpdateProduct)
                        }
                    }
                };
            }

            var product = products.AllProducts.FirstOrDefault(x => x.Id == request.Id);

            if (product == null)
            {
                return new ProductResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "004",
                            Message = $"Product, with ID = {request.Id}, not found!",
                            Target = nameof(UpdateProduct)
                        }
                    }
                };

            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                product.Name = request.Name;
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                product.Description = request.Description;
            }

            if (request.Price != null)
            {
                product.Price = request.Price;
            }

            if (!string.IsNullOrEmpty(request.Photo))
            {
                product.Photo = request.Photo;
            }

            if (request.Inventory != null)
            {
                product.Inventory = request.Inventory;
            }


            using (StreamWriter writer = File.CreateText(FileName))
            {
                string output = JsonConvert.SerializeObject(products);
                writer.Write(output);
            }

            return new ProductResponse
            {
                Sucsess = true,
                Product = product
            };
        }

        public ProductResponse DeleteProduct(string id)
        {
            var products = GetAllProducts();
            if (products == null)
            {
                return new ProductResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "003",
                            Message = "File products.json not found!",
                            Target = nameof(DeleteProduct)
                        }
                    }
                };
            }

            var product = products.AllProducts.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return new ProductResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "004",
                            Message = $"Product, with ID = {id}, not found!",
                            Target = nameof(DeleteProduct)
                        }
                    }
                };
            }

            products.AllProducts.Remove(product);

            using (StreamWriter writer = File.CreateText(FileName))
            {
                string output = JsonConvert.SerializeObject(products);
                writer.Write(output);
            }

            return new ProductResponse
            {
                Sucsess = true
            };
        }

        public ProductResponse GetProductById(string id)
        {
            var products = GetAllProducts();

            if (products == null)
            {
                return new ProductResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "003",
                            Message = "File products.json not found!",
                            Target = nameof(GetProductById)
                        }
                    }
                };

            }

            var product = products.AllProducts.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return new ProductResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "004",
                            Message = $"Product, with ID = {id}, not found!",
                            Target = nameof(DeleteProduct)
                        }
                    }
                };
            }

            return new ProductResponse
            {
                Sucsess = true,
                Product = product
            };
        }
    }
}
