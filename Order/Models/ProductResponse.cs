namespace Order.Models
{
    public class ProductResponse : Response
    {
        public ProductResponse()
        {
        }

        public ProductResponse(bool success)
        {
            Success = success;
        }

        public ProductResponse(Error error)
        {
            Success = false;
            Errors = new List<Error> { error };
        }

        public ProductResponse(Product product)
        {
            Success = true;
            Product = product;
        }

        public ProductResponse(Products products)
        {
            Success = true;
            Products = products;
        }

        public Product? Product { get; set; }
        public Products? Products { get; set; }
    }
}
