using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TypeDeliveryController : Controller
    {
        private readonly ITypeDeliveryService _TypeDeliveryService;
        public TypeDeliveryController(ITypeDeliveryService typeDeliveryService)
        {
            _TypeDeliveryService = typeDeliveryService;
        }

        // GET: TypeDeliveryController
        public ActionResult Index()
        {
            var typeDeliveries = _TypeDeliveryService.GetAll();

            return View(typeDeliveries);
        }

        // GET: TypeDeliveryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TypeDeliveryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeDeliveryController/Create
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

        // GET: TypeDeliveryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TypeDeliveryController/Edit/5
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

        // GET: TypeDeliveryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TypeDeliveryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            OperationResult result = _TypeDeliveryService.Delete(id);
            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.Message;
            }
            else
            {
                ViewData["Success"] = "Se ha eliminado al tipo de envio!";
            }
            var typeDeliveries = _TypeDeliveryService.GetAll();
            return View("Index", typeDeliveries);
        }
    }
}
