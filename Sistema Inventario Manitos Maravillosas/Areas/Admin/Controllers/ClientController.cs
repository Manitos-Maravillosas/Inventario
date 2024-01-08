using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ClientController : Controller
    {

        private readonly IClientService _clientService;
        private readonly IAddressService _addressService;
        public ClientController(IClientService clientService, IAddressService addressService)
        {
            _clientService = clientService;
            _addressService = addressService;
        }


        // GET: Admin/Client
        public ActionResult Index()
        {
            var clients = _clientService.GetAll();
            return View(clients);
        }

        // GET: Admin/Client/Create
        public ActionResult Create()
        {
            return View();
        }
        public JsonResult GetDepartmentNames()
        {
            var departmentNames = _addressService.GetDepartmentNames();
            return Json(departmentNames);
        }

        public JsonResult GetCitiesByDepartment(string departmentName)
        {
            var cityNames = _addressService.GetCitiesByDepartmentName(departmentName);
            return Json(cityNames);
        }

        // POST: Admin/Client/Create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                OperationResult result = _clientService.Add(client);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Cliente agregado correctamente!";

            }
            return View(client);
        }

        // POST: ClientController/Edit/5
        public ActionResult Edit(string id)
        {
            Client client = _clientService.GetById(id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                OperationResult result = _clientService.Update(client);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Se ha modificado los datos del cliente!";

            }
            return View(client);
        }

        public ActionResult Delete(string id)
        {

            OperationResult result = _clientService.Delete(id);
            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.Message;
            }
            else
            {
                ViewData["Success"] = "Se ha eliminado al Cliente!";
            }
            var clients = _clientService.GetAll();
            return View("Index", clients);
        }


    }
}
