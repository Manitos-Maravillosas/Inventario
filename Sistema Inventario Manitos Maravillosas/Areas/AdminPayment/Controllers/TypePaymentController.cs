using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Inventario_Manitos_Maravillosas.Areas.AdminPayment.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Areas.AdminPayment.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.AdminPayment.Controllers
{
    [Area("AdminPayment")]

    [Authorize]
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

            LoadSelect();
            return View();
        }

        // POST: TypePaymentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TypePaymentxCoin typePayment)
        {
            typePayment.CoinName = _CoinService.GetNameFromDescription(typePayment.CoinDescription);
            
                OperationResult result = _TypePaymentService.Add(typePayment);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Tipo de pago agregado correctamente!";

            LoadSelect();
            var typePayments = _TypePaymentService.GetAll();
            return View("Index", typePayments);
        }

        // GET: TypePaymentController/Edit/5
        public ActionResult Edit(int id)
        {
            TypePaymentxCoin typePayment = _TypePaymentService.GetById(id);
            if (typePayment == null)
            {
                return NotFound();
            }

            LoadSelect();
            return View(typePayment);
        }

        // POST: TypePaymentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TypePaymentxCoin typePayment)
        {
            typePayment.CoinName = _CoinService.GetNameFromDescription(typePayment.CoinDescription);
            if (ModelState.IsValid)
            {
                OperationResult result = _TypePaymentService.Update(typePayment);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Se ha modificado los datos del tipo de pago!";
            }  
               
            LoadSelect();
            var typePayments = _TypePaymentService.GetAll();
            return View("Index", typePayments);
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
            LoadSelect();
            var typePayments = _TypePaymentService.GetAll();
            return View("Index", typePayments);
        }
        public void LoadSelect()
        {

            
                // Creating a list of coin SelectListItem
                List<Coin> coins = _CoinService.GetALlCoins();
                var selectCoin = new List<SelectListItem>();
                foreach (var item in coins)
                {
                    selectCoin.Add(new SelectListItem
                    {
                        Value = item.IdCoin.ToString(),
                        Text = item.Description + " " + item.Name
                    });
                }
                ViewBag.CoinSelect = selectCoin;


                // Creating a list of typePayment SelectListItem
                List<TypePayment> typePayment = _TypePaymentService.GetAllTypePayments();
                var selectTypePayment = new List<SelectListItem>();
                foreach (var item in typePayment)
                {
                    selectTypePayment.Add(new SelectListItem
                    {
                        Value = item.IdTypePayment.ToString(),
                        Text = item.Name
                    });
                }
                ViewBag.TypePaymentSelect = selectTypePayment;

            
        }
    }

    
}
