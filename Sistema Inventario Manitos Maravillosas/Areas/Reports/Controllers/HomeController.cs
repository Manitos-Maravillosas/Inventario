using Microsoft.AspNetCore.Mvc;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
