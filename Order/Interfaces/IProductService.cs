using Order.Models;

namespace Order.Interfaces
{
    public interface IProductService
    {
        void SetFileName(string filename);
        Products GetAllProducts();
        ProductResponse AddNewProduct(Product product);
        ProductResponse UpdateProduct(Product product, int? quqntity);
        ProductResponse DeleteProduct(string id);
        ProductResponse GetProductById(string id);
    }
}
