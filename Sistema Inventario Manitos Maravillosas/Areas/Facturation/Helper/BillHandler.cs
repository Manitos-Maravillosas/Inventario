using Newtonsoft.Json;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Helper
{
    public class BillHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IClientService _clientService;
        private Bill bill;
        public BillHandler(IHttpContextAccessor httpContextAccessor, IClientService clientService)
        {
            _httpContextAccessor = httpContextAccessor;
            _clientService = clientService;
        }



        //-------------------------------------------------------------------------------------//
        //                           Bill Handler                                                 //
        //-------------------------------------------------------------------------------------//

        public CartXProduct FindProductById(string id)
        {
            if (bill == null)
                bill = GetBill();
            foreach (var item in bill.CartXProducts)
            {
                if (item.IdProduct == id)
                {
                    return item; // Return the reference to the found product
                }
            }
            return null; // Return null if no product is found
        }


        public void addProductToCartXBill(ProductFacturation product)
        {
            var flagProductFound = false;
            if (bill.CartXProducts.Count == 0)
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

            updatePriceBill();
            SaveBill();
        }

        public bool RemoveProductFromCartXBill(string IdProduct)
        {
            if (bill == null)
                bill = GetBill();
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
                            updatePriceBill();
                            SaveBill();
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new CustomDataException("Error Message", e);
            }

            return flagProductFound;

        }

        public void UpdateProductSubtotalPrice(CartXProduct cartXProduct, int quantity)
        {            
            cartXProduct.Quantity = quantity;
            cartXProduct.SubTotal = cartXProduct.Quantity * cartXProduct.Price;

            updatePriceBill();
            SaveBill();
        }
        public void updatePriceBill()
        {
            //Update bill
            bill.SubTotal = bill.CartXProducts.Sum(x => x.SubTotal);
            bill.TotalCost = bill.SubTotal - (bill.SubTotal * (bill.PercentDiscount / 100));
            SaveBill();
        }

        public void updateClientBill(string id)
        {
            if (bill == null)
                bill = GetBill();
            bill.IdClient = id;
            bill.Client = _clientService.GetById(id);
            updatePriceBill();
            SaveBill();
        }

        public Bill GetBill()
        {
            var sessionData = _httpContextAccessor.HttpContext.Session.GetString("Bill");
            return string.IsNullOrEmpty(sessionData) ? new Bill() : JsonConvert.DeserializeObject<Bill>(sessionData);
        }

        public void SaveBill()
        {
            if (bill == null)
                bill = GetBill();
            var sessionData = JsonConvert.SerializeObject(bill);
            _httpContextAccessor.HttpContext.Session.SetString("Bill", sessionData);
        }
    }
}
