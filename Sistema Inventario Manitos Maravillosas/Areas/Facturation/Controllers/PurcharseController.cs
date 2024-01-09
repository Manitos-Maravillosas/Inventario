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
    public class PurcharseController : Controller
    {
        private readonly IProductService _productService;
        private readonly IClientService _clientService;

        private List<Product> cartProducts = new List<Product>();
        private List<Bill> bills = new List<Bill>();

        public PurcharseController(IProductService productService, IClientService clientService)
        {
            _productService = productService;
            _clientService = clientService;
        }

        // GET: FacturationController
        public ActionResult Index()
        {
            List<Client> clients = _clientService.GetAll();

            return View(clients);
        }

        // POST: Facturation/Purcharse/AddProductToCart
        [HttpPost]
        public IActionResult AddProductToCart(string id, int quantity = 1)
        {
            try
            {
                var product = _productService.GetStockById(id, quantity);
                if (product != null && product.Stock > 0)
                {
                    var productDto = new ProductDto
                    {
                        IdProduct = product.IdProduct,
                        Name = product.ProductName,
                        Stock = product.Stock,
                        Price = product.Cost,
                        Description = product.Description,
                        Status = product.Status
                        // Map other properties as needed
                    };

                    // Add the new product to the cart
                    addProductToCartXBill(product);

                    updatePriceBill();
                    Bill asdf = GetBill();
                    return PartialView("_tableProducts", GetBill());

                    //return Json(new { success = true, message = "Product added to cart successfully.", data = productDto });

                }
                else
                {
                    return Json(new { success = false, message = "Product not available." });
                }
            }
            catch (CustomDataException ex)
            {
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
            }
        }

        private void addProductToCartXBill(Product product)
        {
            Bill bill = GetBill();

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
                        break;
                    }
                }
            }

            SaveBill(bill);
        }

        private void updatePriceBill()
        {
            Bill bill = GetBill();
           
            //Update bill
            bill.SubTotal = bill.CartXProducts.Sum(x => x.SubTotal);
            bill.TotalCost = bill.SubTotal - (bill.SubTotal * (bill.PercentDiscount / 100));
            SaveBill(bill);
        }

        private Bill GetBill()
        {
            var sessionData = HttpContext.Session.GetString("Bill");
            return string.IsNullOrEmpty(sessionData)
                ? new Bill()
                : JsonConvert.DeserializeObject<Bill>(sessionData);
        }

        private void SaveBill(Bill bill)
        {
            var sessionData = JsonConvert.SerializeObject(bill);
            HttpContext.Session.SetString("Bill", sessionData);
        }
        [HttpGet]
        public IActionResult GetClients()
        {
            try
            {
                List<Client> clients= _clientService.GetAll();
                if (clients.Count != 0)
                {
                    
                    return Json(new { success = true, message = "Product added to cart successfully."});

                }
                else
                {
                    return Json(new { success = false, message = "Product not available." });
                }
            }
            catch (CustomDataException ex)
            {
                return Json(new { success = false, message = ex.Message, innerExeption = ex.InnerException });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A general error occurred in ProductService.", ex);
            }
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
                ViewData["Success"] = "Cliente agregado correctamente!";

            }
            return RedirectToAction("Index");
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
