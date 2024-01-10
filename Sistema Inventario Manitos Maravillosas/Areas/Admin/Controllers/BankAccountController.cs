using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BankAccountController : Controller
    {
        private readonly IBankAccountService _BankAccountService;
        private readonly ICoinService _CoinService;
        public BankAccountController(IBankAccountService bankAccountService, ICoinService coinService)
        {
            _BankAccountService = bankAccountService;
            _CoinService = coinService;
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
            return View();
        }

        // POST: BankAccountController/Create
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

        // GET: BankAccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BankAccountController/Edit/5
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
