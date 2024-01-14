﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Newtonsoft.Json;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Helper;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Controllers
{
    [Area("Facturation")]
    [Authorize]
    public class PurchaseController : Controller
    {
        private readonly IProductService _productService;
        private readonly IClientService _clientService;
        private readonly BillHandler _billHandler;

        public PurchaseController(IProductService productService, IClientService clientService,BillHandler billHandler)
        {
            
            _productService = productService;
            _clientService = clientService;
            _billHandler = billHandler;
        }

        // GET: FacturationController
        public ActionResult Index()
        {
            List<Client> clients = _clientService.GetAll();
            var sessionData = HttpContext.Session.GetString("Bill");
            if (!string.IsNullOrEmpty(sessionData))
            {
                ViewData["isBill"] = true;
                Bill b = _billHandler.GetBill();
                ViewData["bill"] = b;
                if (b.Client != null)
                {
                    
                    ViewData["isClient"] = true;
                }
            }

            return View(clients);
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
                        return PartialView("_tableProducts", _billHandler.GetBill());
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
        public void AssingClientToBill(string id)
        {
            _billHandler.updateClientBill(id);

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
            Bill z = _billHandler.GetBill();
            var sessionData = HttpContext.Session.GetString("Bill");
            return RedirectToAction("Index");
        }

        

    }

}