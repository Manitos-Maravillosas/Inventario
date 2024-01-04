using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using System.Data.SqlClient;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Data.Services;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Controllers
{
    [Area("Facturation")]
    public class PurcharseController : Controller
    {
        private readonly IProductService _productService;

        private List<Product> cartProducts = new List<Product>();

        public PurcharseController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: FacturationController
        public ActionResult Index()
        {

            ViewBag.Title = "Facturaasdfasdftion";
            return View();
        }

        // POST: Facturation/Purcharse/AddProductToCart
        [HttpPost]
        public IActionResult AddProductToCart(string id, int quantity = 1)
        {
            try
            {
                var product = _productService.GetStockById(id, quantity);
                if (product != null && product.Stock > 0)
                {
                    var productDto = new ProductDto
                    {
                        IdProduct = product.IdProduct,
                        Name = product.ProductName,
                        Stock = product.Stock,
                        Price = product.Cost,
                        Description = product.Description,
                        Status = product.Status
                        // Map other properties as needed
                    };
                    cartProducts.Add(product);
                    return Json(new { success = true, message = "Product added to cart successfully.", data = productDto });

                }
                else
                {
                    return Json(new { success = false, message = "Product not available." });
                }
            }
            catch (CustomDataException ex)
            {
                return Json(new { success = false, message = ex.Message, innerExeption = ex.InnerException });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A general error occurred in ProductService.", ex);
            }
        }




    }

    public class ProductDto
    {
        public string IdProduct { get; set; }
        public string Name { get; set; }
        public float Stock { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        // Add any other properties needed by the client
    }

}
