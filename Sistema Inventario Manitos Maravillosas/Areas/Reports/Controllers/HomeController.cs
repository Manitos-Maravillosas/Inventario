using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Report(ReportsViewModel reportsView)
        {
            if (ModelState.IsValid)
            {
                //TODO: Generate report

            }
            return RedirectToAction("Index");
        }
    }
}
