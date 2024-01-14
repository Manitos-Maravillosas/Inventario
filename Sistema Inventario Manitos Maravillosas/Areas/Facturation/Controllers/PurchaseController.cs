using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Newtonsoft.Json;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Controllers
{
    [Area("Facturation")]
    [Authorize]
    public class PurchaseController : Controller
    {
        private readonly IProductService _productService;
        private readonly IClientService _clientService;


        public PurchaseController(IProductService productService, IClientService clientService)
        {
            _productService = productService;
            _clientService = clientService;
        }

        // GET: FacturationController
        public ActionResult Index()
        {
            List<Client> clients = _clientService.GetAll();
            var sessionData = HttpContext.Session.GetString("Bill");
            if (!string.IsNullOrEmpty(sessionData))
            {
                ViewData["isBill"] = true;
                Bill b = GetBill();
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
        public IActionResult AddProductToCart(string id, int quantity = 1)
        {
            try
            {
                var product = _productService.GetStockById(id, quantity);
                if (product != null && product.Stock > 0)
                {

                    // Add the new product to the cart
                    addProductToCartXBill(product);

                    updatePriceBill();
                    return PartialView("_tableProducts", GetBill());

                    //return Json(new { success = true, message = "Product added to cart successfully.", data = productDto });

                }
                else
                {
                    return Json(new { success = false, message = "No hay producto disponible" });
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
           if (RemoveProductFromCartXBill(id))
            {
                return PartialView("_tableProducts", GetBill());
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
            updateClientBill(id);

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
                    updateClientBill(client.Id);
                }
                               

            }
            Bill z = GetBill();
            var sessionData = HttpContext.Session.GetString("Bill");
            return RedirectToAction("Index");
        }

        //-------------------------------------------------------------------------------------//
        //                           Bill Handler                                                 //
        //-------------------------------------------------------------------------------------//
        private void addProductToCartXBill(Product product)
        {
            Bill bill = GetBill();
            var flagProductFound = false;
            if (bill.CartXProducts == null)
            {
                bill.CartXProducts = new List<CartXProduct>();
            }
            else if (bill.CartXProducts.Count == 0)
            {
                bill.CartXProducts.Add(new CartXProduct
                {
                    IdProduct = product.IdProduct,
                    Quantity = 1,
                    Cost = product.Cost,
                    Price = product.Price,
                    SubTotal = product.Price,
                    Product = product
                });
            }
            else
            {
                foreach (var item in bill.CartXProducts)
                {
                    if (item.IdProduct == product.IdProduct)
                    {
                        item.Quantity += 1;
                        item.SubTotal = item.Quantity * item.Price;
                        flagProductFound = true;
                        break;
                    }
                }

                if (!flagProductFound)
                {
                    bill.CartXProducts.Add(new CartXProduct
                    {
                        IdProduct = product.IdProduct,
                        Quantity = 1,
                        Cost = product.Cost,
                        Price = product.Price,
                        SubTotal = product.Price,
                        Product = product
                    });
                }
            }

            SaveBill(bill);
        }

        private bool RemoveProductFromCartXBill(string IdProduct)
        {
            Bill bill = GetBill();
            var flagProductFound = false;


            try
            {
                if (bill != null)
                {
                    foreach (var item in bill.CartXProducts)
                    {
                        if (item.IdProduct == IdProduct)
                        {
                            bill.CartXProducts.Remove(item);
                            flagProductFound = true;
                            SaveBill(bill);
                            break;
                        }
                    }
                }
            }catch (Exception e)
            {
                throw new CustomDataException("Error Message", e);
            }

            return flagProductFound;
            
        }

        private void updatePriceBill()
        {
            Bill bill = GetBill();

            //Update bill
            bill.SubTotal = bill.CartXProducts.Sum(x => x.SubTotal);
            bill.TotalCost = bill.SubTotal - (bill.SubTotal * (bill.PercentDiscount / 100));
            SaveBill(bill);
        }

        private void updateClientBill(string id)
        {
            Bill bill = GetBill();
            bill.IdClient = id;
            bill.Client = _clientService.GetById(id);
            SaveBill(bill);
        }

        private Bill GetBill()
        {
            var sessionData = HttpContext.Session.GetString("Bill");
            return string.IsNullOrEmpty(sessionData)  ? new Bill() : JsonConvert.DeserializeObject<Bill>(sessionData);
        }

        private void SaveBill(Bill bill)
        {
            var sessionData = JsonConvert.SerializeObject(bill);
            HttpContext.Session.SetString("Bill", sessionData);
        }

    }

    

    public class ProductDto
    {
        public string IdProduct { get; set; }
        public string Name { get; set; }
        public float Stock { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        // Add any other properties needed by the client
    }

}
