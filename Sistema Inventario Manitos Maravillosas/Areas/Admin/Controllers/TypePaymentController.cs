using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TypePaymentController : Controller
    {
        private readonly ITypePaymentService _TypePaymentService;
        private readonly ICoinService _CoinService;
        public TypePaymentController(ITypePaymentService typePaymentService, ICoinService coinService)
        {
            _TypePaymentService = typePaymentService;
            _CoinService = coinService;
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
            var coinDescriptions = _CoinService.GetCoinDescriptions();
            ViewBag.CoinDescriptions = new SelectList(coinDescriptions);
            return View();
        }

        // POST: TypePaymentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TypePayment typePayment)
        {
            if (ModelState.IsValid)
            {
                OperationResult result = _TypePaymentService.Add(typePayment);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Tipo de pago agregado correctamente!";

            }
            ViewBag.CoinDescriptions = new SelectList(_CoinService.GetCoinDescriptions());
            return View(typePayment);
        }

        // GET: TypePaymentController/Edit/5
        public ActionResult Edit(int id)
        {
            TypePayment typePayment = _TypePaymentService.GetById(id);
            if (typePayment == null)
            {
                return NotFound();
            }
            ViewBag.CoinDescriptions = new SelectList(_CoinService.GetCoinDescriptions());
            return View(typePayment);
        }

        // POST: TypePaymentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TypePayment typePayment)
        {
            if (ModelState.IsValid)
            {
                OperationResult result = _TypePaymentService.Update(typePayment);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Se ha modificado los datos del tipo de pago!";

            }
            return View();
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
            var typePayments = _TypePaymentService.GetAll();
            return View("Index", typePayments);
        }
    }
}
