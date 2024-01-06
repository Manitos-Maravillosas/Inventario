using Microsoft.AspNetCore.Mvc;

namespace Sistema_Inventario_Manitos_Maravillosas.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/Index/{code:int}")]
        public IActionResult Index(int code)
        {
            if (code == 404)
            {
                return View("NotFound");
            }
            else
            {
                return View("Error");
            }
        }
    }
}
