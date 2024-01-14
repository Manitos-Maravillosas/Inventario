using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Inventory.Models;
using Sistema_Inventario_Manitos_Maravillosas.Data;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class ProductCategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(AppDbContext context, IProductCategoryService productCategoryService)
        {
            _context = context;
            _productCategoryService = productCategoryService;
        }

        // GET: Inventory/ProductCategories
        public async Task<IActionResult> Index()
        {
            var productCategories = _productCategoryService.GetAll();

            return View(productCategories);
        }

        // GET: Inventory/ProductCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductCategories == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories
                .FirstOrDefaultAsync(m => m.IdProductCategory == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // GET: Inventory/ProductCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inventory/ProductCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                OperationResult result = _productCategoryService.Add(productCategory);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Categoria agregada correctamente!";

            }
            return View();
        }

        // GET: Inventory/ProductCategories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ProductCategory employee = _productCategoryService.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Inventory/ProductCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                OperationResult result = _productCategoryService.Update(productCategory);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Se han modificado los datos de la categoria!";

            }
            return View();
        }

        // GET: Inventory/ProductCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductCategories == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories
                .FirstOrDefaultAsync(m => m.IdProductCategory == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // POST: Inventory/ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductCategories == null)
            {
                return Problem("Entity set 'AppDbContext.ProductCategories'  is null.");
            }
            var productCategory = await _context.ProductCategories.FindAsync(id);
            if (productCategory != null)
            {
                _context.ProductCategories.Remove(productCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCategoryExists(int id)
        {
            return (_context.ProductCategories?.Any(e => e.IdProductCategory == id)).GetValueOrDefault();
        }
    }
}
