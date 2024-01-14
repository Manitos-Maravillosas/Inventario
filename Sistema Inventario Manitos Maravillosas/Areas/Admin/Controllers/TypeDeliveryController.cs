using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
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
        public ActionResult Create(TypeDelivery typeDelivery)
        {
            if (ModelState.IsValid)
            {
                OperationResult result = _TypeDeliveryService.Add(typeDelivery);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Tipo de envio agregado correctamente!";

            }
            
            return View(typeDelivery);
        }

        // GET: TypeDeliveryController/Edit/5
        public ActionResult Edit(int id)
        {
            TypeDelivery typeDelivery = _TypeDeliveryService.GetById(id);
            if (typeDelivery == null)
            {
                return NotFound();
            }
            return View(typeDelivery);
        }

        // POST: TypeDeliveryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TypeDelivery typeDelivery)
        {
            if (ModelState.IsValid)
            {
                OperationResult result = _TypeDeliveryService.Update(typeDelivery);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Se ha modificado los datos del tipo de envio!";

            }
            return View();
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
