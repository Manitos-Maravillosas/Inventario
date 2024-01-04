using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Diagnostics;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller

    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: EmployeeController
        public ActionResult Index()
        {
            var employees = _employeeService.GetAll();
            
            return View("~/Areas/Admin/Views/Emp/Index.cshtml", employees);
        }


        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            var businessNames = _employeeService.GetBusinessNames();
            var userEmails = _employeeService.GetUserEmails(); 
            ViewBag.BusinessNames = new SelectList(businessNames);
            ViewBag.UserEmails = new SelectList(userEmails);

            return View("~/Areas/Admin/Views/Emp/Create.cshtml");
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {            

            if (ModelState.IsValid)
            {
                OperationResult result = _employeeService.Add(employee);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Empleado agregado correctamente!";
                
            }
            ViewBag.BusinessNames = new SelectList(_employeeService.GetBusinessNames());
            ViewBag.UserEmails = new SelectList(_employeeService.GetUserEmails());
            return View("~/Areas/Admin/Views/Emp/Create.cshtml");
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(string id)
        {
            Employee employee = _employeeService.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewBag.BusinessNames = new SelectList(_employeeService.GetBusinessNames());
            ViewBag.UserEmails = new SelectList(_employeeService.GetUserEmails());
            return View("~/Areas/Admin/Views/Emp/Edit.cshtml",employee);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                OperationResult result = _employeeService.Update(employee);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Se ha modificado los datos del empleado!";

            }
            return View("~/Areas/Admin/Views/Emp/Edit.cshtml");
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(string id)
        {
            OperationResult result = _employeeService.Delete(id);
            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.Message;
            }
            else
            {
                ViewData["Success"] = "Se ha eliminado al Empleado!";
            }
            var employees = _employeeService.GetAll();
            return View("~/Areas/Admin/Views/Emp/Index.cshtml", employees);
        }

        
    }
}
