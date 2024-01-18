using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Helper;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Controllers
{
    [Area("Facturation")]
    public class BillController : Controller
    {
        private readonly IBillService _BillService;
        private readonly IClientService _clientService;

        private readonly ITypeDeliveryService _typeDeliveryService;

        private readonly IEmployeeService _employeeService;
        private readonly IBusinessService _businessService;
        private readonly IDeliveryService _deliveryService;
        private readonly BillHandler _billHandler;
        public BillController(IBillService billService, BillHandler billHandler, IClientService clientService, ITypeDeliveryService typeDeliveryService,
            IDeliveryService deliveryService,   IEmployeeService employeeService, IBusinessService businessService)
        {
            _BillService = billService;
            _billHandler = billHandler;
            _clientService = clientService;
            _typeDeliveryService = typeDeliveryService;
            _deliveryService = deliveryService;
            _employeeService = employeeService;
            _businessService = businessService;
        }
        // GET: BillController
        public ActionResult Index()
        {
            var bills = _BillService.GetAll();

            return View(bills);
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            
            Bill bill = _BillService.GetBillById(id);
            bill.Employee = _employeeService.GetById(bill.IdEmployee);
            bill.Client = _clientService.GetById(bill.IdClient);


            //typeDelivery
            List<TypeDelivery> typeDeliverries = _typeDeliveryService.GetAll();
            // Creating a list of SelectListItem
            var selectTypeDeliveriesList = new List<SelectListItem>();
            foreach (var item in typeDeliverries)
            {
                selectTypeDeliveriesList.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }

            //Compnies trans
            List<CompanyTrans> companies = _deliveryService.GetAllCompanies();
            // Creating a list of SelectListItem
            var selectCompaniesList = new List<SelectListItem>();
            foreach (var item in companies)
            {
                selectCompaniesList.Add(new SelectListItem
                {
                    Value = item.IdCompanyTrans.ToString(),
                    Text = item.Name
                });
            }
            ViewBag.CompanyTrans = selectCompaniesList;
            ViewBag.TypeDelivery = selectTypeDeliveriesList;

            return View(bill);
        }



    }
}
