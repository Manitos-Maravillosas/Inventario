using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BankAccountController : Controller
    {
        private readonly IBankAccountService _BankAccountService;
        private readonly ICoinService _CoinService;
        private readonly ITypePaymentService _TypePaymentService;
        public BankAccountController(IBankAccountService bankAccountService, ICoinService coinService, ITypePaymentService typePaymentService)
        {
            _BankAccountService = bankAccountService;
            _CoinService = coinService;
            _TypePaymentService = typePaymentService;
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
            var coinDescriptions = _CoinService.GetCoinDescriptions();
            ViewBag.CoinDescriptions = new SelectList(coinDescriptions);

            var typePaymentNames = _TypePaymentService.GetTypePayments();
            ViewBag.TypePaymentNames = new SelectList(typePaymentNames);

            var bankNames = _BankAccountService.GetBankNames();
            ViewBag.BankNames = new SelectList(bankNames);
            return View();
        }

        // POST: BankAccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                OperationResult result = _BankAccountService.Add(bankAccount);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Cuenta de Banco agregada correctamente!";

            }
            ViewBag.CoinDescriptions = new SelectList(_CoinService.GetCoinDescriptions());
            ViewBag.TypePaymentNames = new SelectList(_TypePaymentService.GetTypePayments());
            ViewBag.BankNames = new SelectList(_BankAccountService.GetBankNames());
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
            ViewBag.CoinDescriptions = new SelectList(_CoinService.GetCoinDescriptions());
            ViewBag.TypePaymentNames = new SelectList(_TypePaymentService.GetTypePayments());
            ViewBag.BankNames = new SelectList(_BankAccountService.GetBankNames());
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
    }
}
