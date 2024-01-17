using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Supplier.Controllers
{
    [Area("Supplier")]
    public class PurchaseProviderController : Controller
    {
        private readonly IPurchaseProviderService _PurchaseProviderService;
        public PurchaseProviderController(IPurchaseProviderService purchaseProviderService)
        {
            _PurchaseProviderService = purchaseProviderService;
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
            return View();
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
