using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TypePaymentController : Controller
    {
        private readonly ITypePaymentService _TypePaymentService;
        public TypePaymentController(ITypePaymentService typePaymentService)
        {
            _TypePaymentService = typePaymentService;
        }

        // GET: TypePaymentController
        public ActionResult Index()
        {
            var typePayments = _TypePaymentService.GetAll();

            return View(typePayments);
        }

        // GET: TypePaymentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TypePaymentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypePaymentController/Create
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

        // GET: TypePaymentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TypePaymentController/Edit/5
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

        // GET: TypePaymentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TypePaymentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            OperationResult result = _TypePaymentService.Delete(id);
            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.Message;
            }
            else
            {
                ViewData["Success"] = "Se ha eliminado al tipo de pago!";
            }
            var clients = _TypePaymentService.GetAll();
            return View("Index", clients);
        }
    }
}
