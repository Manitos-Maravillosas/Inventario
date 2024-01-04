using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Models.Inventory;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using System.Data.SqlClient;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;

namespace Sistema_Inventario_Manitos_Maravillosas.Controllers.Facturation
{
    public class FacturationController : Controller
    {
        private readonly IProductService _productService;

        private List<Product> cartProducts = new List<Product>();

        public FacturationController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: FacturationController
        public ActionResult Index()
        {

            ViewBag.Title = "Facturaasdfasdftion";
            return View();
        }

        // POST: Facturation/AddProductToCart
        [HttpPost]
        public IActionResult AddProductToCart(string id, int quantity = 1)
        {
            try
            {
                var product = _productService.GetStockById(id, quantity);
                if (product != null && product.Stock > 0)
                {
                    cartProducts.Add(product);
                    return Json(new { success = true, message = "Product added to cart successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Product not available." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, innerExeption = ex.InnerException });
            }
        }






        // GET: FacturationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FacturationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FacturationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FacturationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FacturationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FacturationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FacturationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
