using Microsoft.AspNetCore.Mvc;
using Order.Interfaces;
using Order.Models;
using Order.Models.Account;

namespace Order.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly User _user;

        public ProductController(IProductService productService, User user)
        {
            _productService = productService;
            _productService.SetFileName("products.json");
            _user = user;
        }

        [HttpGet]
        [Route("allProducts")]
        public ActionResult GetAllProducts()
        {
            var request = _productService.GetAllProducts();

            return View(request);
        }

        [HttpGet]
        [Route("addNewProduct")]
        public ActionResult AddNewProduct(string name, string desc, decimal price, string photo, int inventory)
        {
            if (_user.MemberType == MemberType.Administrator)
            {
                var product = new Product
                {
                    Name = name,
                    Description = desc,
                    Price = price,
                    Photo = photo,
                    Inventory = inventory
                };
                var response = _productService.AddNewProduct(product);

                return View(response);
            }

            return View("Access deniedS");
        }

        [HttpGet]
        [Route("update")]
        public ActionResult UpdateProduct(string id, string? name, string? desc, decimal price, string? photo, int inventory)
        {
            if (_user.MemberType == MemberType.Administrator)
            {
                var product = new Product
                {
                    Id = id,
                    Name = name,
                    Description = desc,
                    Price = price,
                    Photo = photo,
                    Inventory = inventory
                };
                var response = _productService.UpdateProduct(product);

                return View(response);
            }

            return View("Access denied");
        }

        [HttpGet]
        [Route("delete")]
        public ActionResult DeleteProduct(string id)
        {
            if (_user.MemberType == MemberType.Administrator)
            {
                var response = _productService.DeleteProduct(id);

                return View(response);
            }

            return View("Access denied");
        }

        [HttpGet]
        [Route("getProduct")]
        public ActionResult GetProductById(string id)
        {
            var request = _productService.GetProductById(id);

            return View(request);
        }
    }
}
