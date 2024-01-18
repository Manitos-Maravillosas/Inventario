using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Helper;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Globalization;
using IProductServiceFacturation = Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Data.Services.IProductServiceFacturation;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Controllers
{
    [Area("Facturation")]
    [Authorize]
    public class PurchaseController : Controller
    {
        private readonly IProductServiceFacturation _productService;
        private readonly IClientService _clientService;
        private readonly ITypeDeliveryService _typeDeliveryService;
        private readonly IDeleveryService _deleveryService;
        private readonly IEmployeeService _employeeService;
        private readonly BillHandler _billHandler;



        public PurchaseController(IProductServiceFacturation productService, IClientService clientService, ITypeDeliveryService typeDeliveryService,
            IDeleveryService deleveryService, IEmployeeService employeeService, BillHandler billHandler)

        {

            _productService = productService;
            _clientService = clientService;
            _typeDeliveryService = typeDeliveryService;
            _deleveryService = deleveryService;
            _employeeService = employeeService;
            _billHandler = billHandler;
        }

        // GET: FacturationController
        public ActionResult Index()
        {
            string userEmail = User.Identity.Name;
            Bill b = _billHandler.GetBill();
            _billHandler.AssingEmployee(_employeeService.GetEmployeeByEmail(userEmail));
            b.listClients = _clientService.GetAll();
            var sessionData = HttpContext.Session.GetString("Bill");
            ViewData["isBill"] = true;
            if (b.Client != null)
            {
                ViewData["isClient"] = true;
            }
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
            List<CompanyTrans> companies = _deleveryService.GetAllCompanies();
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

            return View(b);
        }
        //-------------------------------------------------------------------------------------//
        //                           Product                                                  //
        //-------------------------------------------------------------------------------------//

        // POST: Facturation/Purcharse/AddProductToCart
        [HttpPost]
        public IActionResult AddProductToCart(string id, int quantity)
        {
            if (quantity <= 0)
            {
                return Json(new { success = false, message = "La cantidad debe ser mayor a 0" });
            }
            try
            {
                CartXProduct cartXProduct = _billHandler.FindProductById(id);
                if (cartXProduct == null)
                {
                    var product = _productService.GetStockById(id, quantity);
                    if (product != null && product.Stock > 0)
                    {

                        // Add the new product to the cart
                        _billHandler.addProductToCartXBill(product);
                        return PartialView("_tableProducts", _billHandler.GetBill());

                    }
                    else
                    {
                        return Json(new { success = false, message = "No hay producto disponible" });
                    }
                }
                else
                {


                    quantity += cartXProduct.Quantity;
                    var product = _productService.GetStockById(id, quantity);

                    if (product != null)
                    {
                        _billHandler.UpdateProductSubtotalPrice(cartXProduct, quantity);
                        return PartialView("_tableProducts", _billHandler.GetBill());
                    }
                    else
                    {
                        return Json(new { success = false, message = "Ha ocurrido un error inesperado" });
                    }
                    //_billHandler.updatePriceBill();

                }

            }
            catch (CustomDataException ex)
            {
                if (ex.Message == "Sql")
                {
                    return Json(new { success = false, message = ex.InnerException.Message });
                }
                else
                {
                    throw new CustomDataException("An error occurred: " + ex.Message, ex);
                }
            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
            }
        }
        // POST: Facturation/Purcharse/AddProductToCart
        [HttpPost]
        public IActionResult UpdateQuanty(string id, int quantity)
        {
            if (quantity <= 0)
            {
                return Json(new { success = false, message = "La cantidad debe ser mayor a 0" });
            }
            try
            {
                CartXProduct cartXProduct = _billHandler.FindProductById(id);
                if (cartXProduct != null)
                {
                    var product = _productService.GetStockById(id, quantity);

                    if (product != null)
                    {
                        _billHandler.UpdateProductSubtotalPrice(cartXProduct, quantity);
                        //return PartialView("_tableProducts", _billHandler.GetBill());
                        return Json(new { success = true, message = _billHandler.GetBill() });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Ha ocurrido un error inesperado" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Ha ocurrido un error inesperado" });

                }

            }
            catch (CustomDataException ex)
            {
                if (ex.Message == "Sql")
                {
                    return Json(new { success = false, message = ex.InnerException.Message });
                }
                else
                {
                    throw new CustomDataException("An error occurred: " + ex.Message, ex);
                }
            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
            }
        }

        // POST: Facturation/Purcharse/removeProductFromCart
        [HttpPost]
        public IActionResult removeProductFromCart(string id)
        {
            if (_billHandler.RemoveProductFromCartXBill(id))
            {
                return PartialView("_tableProducts", _billHandler.GetBill());
               
            }
            else
            {
                return Json(new { success = false, message = "Product not available." });
            }
        }

        //-------------------------------------------------------------------------------------//
        //                           Client                                                  //
        //-------------------------------------------------------------------------------------//

        [HttpPost]
        public IActionResult AssingClientToBill(string id)
        {
            _billHandler.updateClientBill(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateClient(Client client)
        {
            if (ModelState.IsValid)
            {
                OperationResult result = _clientService.Add(client);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;

                }
                else
                {
                    ViewData["Success"] = "Cliente agregado correctamente!";
                    _billHandler.updateClientBill(client.Id);
                }


            }
            return RedirectToAction("Index");
        }

        //-------------------------------------------------------------------------------------//
        //                           Delevery                                                  //
        //-------------------------------------------------------------------------------------//

        [HttpGet]
        public IActionResult GetTypeDeliveries()
        {
            List<TypeDelivery> deliveries = _typeDeliveryService.GetAll();
            return Json(deliveries);
        }

        [HttpGet]
        public IActionResult GetCompanyTrans()
        {
            List<CompanyTrans> companies = _deleveryService.GetAllCompanies();
            return Json(companies);
        }

        [HttpPost]
        public IActionResult AssingDeliveryToBill(Bill bill)
        {
            _billHandler.UpdateDeliveryBill(bill.deliveryFlag, bill.delivery);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ClearData()
        {
            _billHandler.ClearData();
            return RedirectToAction("Index");
        }


        //-------------------------------------------------------------------------------------//
        //                           Money                                                  //
        //-------------------------------------------------------------------------------------//

        [HttpPost]
        public IActionResult ConvertMoney(int option, string value)
        {
            float valueF = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            //option 658 = USD -> C$
            
            try
            {
                if (option == 658)
                {
                    //store the money in the session
                    HttpContext.Session.SetString("MoneyValue", value);
                    HttpContext.Session.SetString("MoneyId", value);
                    _billHandler.UpdateMoneyBill(valueF, 2);


                }
                //option 12 = C$ -> USD
                else if (option == 12)
                {
                    //store the money in the session
                    HttpContext.Session.SetString("MoneyValue", value);
                    HttpContext.Session.SetString("MoneyId", value);
                    _billHandler.UpdateMoneyBill(valueF, 1);
                }
                return PartialView("_tableProducts", _billHandler.GetBill());           

            }
            catch (CustomDataException ex)
            {
                if (ex.Message == "Sql")
                {
                    return Json(new { success = false, message = ex.InnerException.Message });
                }
                else
                {
                    throw new CustomDataException("An error occurred: " + ex.Message, ex);
                }
            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
            }
        }


    }

}
