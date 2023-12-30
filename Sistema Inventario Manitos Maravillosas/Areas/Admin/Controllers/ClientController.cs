using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Data;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClientController : Controller
    {

        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
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
                ViewData["Success"] = "Client created successfully!";
               
            }
            return View(client); // Return the view with the client model if ModelState is not valid
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
                ViewData["Success"] = "Se ha modificado los datos del Cliente!";

            }
            return View(client); // Return the view with the client model if ModelState is not valid
        }

        public ActionResult Delete(string id)
        {
            
            OperationResult result = _clientService.Delete(id);
            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.Message;
            }
            ViewData["Success"] = "Se ha eliminado al Cliente!";
            return View("Index");
        }





    }
}
