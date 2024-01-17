using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.AdminPayment.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Areas.AdminPayment.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.AdminPayment.Controllers
{
    [Area("AdminPayment")]

    [Authorize]
    public class BankAccountController : Controller
    {
        private readonly IBankAccountService _BankAccountService;
        private readonly ICoinService _CoinService;
        private readonly ITypePaymentService _typePaymentService;
        public BankAccountController(IBankAccountService bankAccountService, ICoinService coinService, ITypePaymentService typePaymentService)
        {
            _BankAccountService = bankAccountService;
            _CoinService = coinService;
            _typePaymentService = typePaymentService;
        }
        // GET: BankAccountController
        public ActionResult Index()
        {
            var bankAccounts = _BankAccountService.GetAll();

            return View(bankAccounts);
        }

        // GET: BankAccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BankAccountController/Create
        public ActionResult Create()
        {
            LoadSelect();
            return View();
        }

        // POST: BankAccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                bankAccount.idTypePaymentxCoin = _typePaymentService.GetIdTypePaymentxCoin(bankAccount.IdTypePayment, bankAccount.IdCoin);
                if (bankAccount.idTypePaymentxCoin != 0)
                {
                    OperationResult result = _BankAccountService.Add(bankAccount);
                    if (!result.Success)
                    {
                        ViewData["ErrorMessage"] = result.Message;
                    }
                    ViewData["Success"] = "Cuenta de Banco agregada correctamente!";
                }
                else
                {
                    ViewData["ErrorMessage"] = "No se puede agregar la cuenta de banco, ya que no existe un tipo de pago para la moneda seleccionada.";
                }
               

            }
            LoadSelect();
            return View(bankAccount);
        }

        // GET: BankAccountController/Edit/5
        public ActionResult Edit(int id)
        {
            BankAccount bankAccount = _BankAccountService.GetById(id);
            if (bankAccount == null)
            {
                return NotFound();
            }
            LoadSelect();
            return View(bankAccount);
        }

        // POST: BankAccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                OperationResult result = _BankAccountService.Update(bankAccount);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Se ha modificado los datos de la cuenta de Banco!";

            }
            return View();
        }

        // GET: BankAccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BankAccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            OperationResult result = _BankAccountService.Delete(id);
            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.Message;
            }
            else
            {
                ViewData["Success"] = "Se ha eliminado la cuenta de banco!";
            }
            var bankAccounts = _BankAccountService.GetAll();
            return View("Index", bankAccounts);
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
                    Text = item.Name
                });
            }
            ViewBag.CoinSelect = selectCoin;


            // Creating a list of typePayment SelectListItem
            List<TypePayment> typePayment = _typePaymentService.GetAllTypePayments();
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

            // Creating a list of bank SelectListItem
            List<Bank> banks = _BankAccountService.GetAllBanks();
            var selectBank = new List<SelectListItem>();
            foreach (var item in banks)
            {
                selectBank.Add(new SelectListItem
                {
                    Value = item.IdBank.ToString(),
                    Text = item.Name
                });
            }
            ViewBag.BankSelect = selectBank;
        }
    }
}
