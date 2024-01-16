using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Supplier.Models;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Supplier.Controllers
{
    [Area("Supplier")]
    public class ProviderController : Controller
    {
        private readonly IProviderService _ProviderService;
        public ProviderController(IProviderService providerService)
        {
            _ProviderService = providerService;
        }
        // GET: ProviderController
        public ActionResult Index()
        {
            var providers = _ProviderService.GetAll();

            return View(providers);
        }

        // GET: ProviderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProviderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProviderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Provider provider)
        {
            if (ModelState.IsValid)
            {
                OperationResult result = _ProviderService.Add(provider);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Proveedor agregado correctamente!";

            }

            return View(provider);
        }

        // GET: ProviderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProviderController/Edit/5
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

        // GET: ProviderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProviderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            OperationResult result = _ProviderService.Delete(id);
            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.Message;
            }
            else
            {
                ViewData["Success"] = "Se ha eliminado al proveedor!";
            }
            var providers = _ProviderService.GetAll();
            return View("Index", providers);
        }
    }
}
