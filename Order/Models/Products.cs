namespace Order.Models
{
    public class Products
    {
        public List<Product> AllProducts { get; set; }

        public void AddProduct(Product product)
        {
            AllProducts.Add(product);
        }

        public void RemoveProduct(Product product)
        {
            AllProducts.Remove(product);
        }
    }
}
