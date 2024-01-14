using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Inventory.Models;
using Sistema_Inventario_Manitos_Maravillosas.Data;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IBusinessService _businessService;

        public ProductController(AppDbContext context, IProductService productService, IProductCategoryService productCategoryService, IBusinessService businessService)
        {
            _context = context;
            _productService = productService;
            _productCategoryService = productCategoryService;
            _businessService = businessService;
        }

        // GET: Inventory/Product
        public async Task<IActionResult> Index()
        {
            var products = _productService.GetAll();

            return View(products);
        }


        // GET: Inventory/Product/Create
        public IActionResult Create()
        {
            var productCategories = _productCategoryService.GetProductCategoryNames();
            var businessNames = _businessService.GetBusinessNames();

            ViewBag.ProductCategories = new SelectList(productCategories);
            ViewBag.BusinessNames = new SelectList(businessNames);

            return View();
        }

        // POST: Inventory/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                OperationResult result = _productService.Add(product);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Producto agregado correctamente!";

            }
            var productCategories = _productCategoryService.GetProductCategoryNames();
            var businessNames = _businessService.GetBusinessNames();

            ViewBag.ProductCategories = new SelectList(productCategories);
            ViewBag.BusinessNames = new SelectList(businessNames);

            return View();
        }

        // GET: Inventory/Product/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            Product product = _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            var productCategories = _productCategoryService.GetProductCategoryNames();
            var businessNames = _businessService.GetBusinessNames();

            ViewBag.ProductCategories = new SelectList(productCategories);
            ViewBag.BusinessNames = new SelectList(businessNames);

            return View(product);
        }

        // POST: Inventory/Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                OperationResult result = _productService.Update(product);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Se han modificado los datos de la categoria!";

            }
            var productCategories = _productCategoryService.GetProductCategoryNames();
            var businessNames = _businessService.GetBusinessNames();

            ViewBag.ProductCategories = new SelectList(productCategories);
            ViewBag.BusinessNames = new SelectList(businessNames);

            return View();
        }


        // POST: Inventory/Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            OperationResult result = _productService.Delete(id);
            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.Message;
            }
            ViewData["Success"] = "Se ha eliminado la categoria!";

            var products = _productService.GetAll();
            return View("Index", products);

        }
    }
}
