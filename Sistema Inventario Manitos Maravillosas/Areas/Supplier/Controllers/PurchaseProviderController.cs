using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Helper;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Inventory.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Supplier.Models;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Supplier.Controllers
{
    [Area("Supplier")]
    public class PurchaseProviderController : Controller
    {
        private readonly IPurchaseProviderService _PurchaseProviderService;
        private readonly IProductService _ProductService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IBusinessService _businessService;
        public PurchaseProviderController(IPurchaseProviderService purchaseProviderService, IProductService productService, IProductCategoryService productCategoryService, IBusinessService businessService)
        {
            _PurchaseProviderService = purchaseProviderService;
            _ProductService = productService;
            _productCategoryService = productCategoryService;
            _businessService = businessService;
        }
        // GET: PurchaseProviderController
        public ActionResult Index()
        {
            var purchases = _PurchaseProviderService.GetAll();
            
            return View(purchases);
        }

        // GET: PurchaseProviderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PurchaseProviderController/Create
        public ActionResult Create()
        {
            PurchaseProvider purchaseProvider = new PurchaseProvider(); 
            purchaseProvider.Products = _ProductService.GetAll();

            return View(purchaseProvider);
        }

        // POST: PurchaseProviderController/Create
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

        // GET: PurchaseProviderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PurchaseProviderController/Edit/5
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

        // GET: PurchaseProviderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PurchaseProviderController/Delete/5
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
