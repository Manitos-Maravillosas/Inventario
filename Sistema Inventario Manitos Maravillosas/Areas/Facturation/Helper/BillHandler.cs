﻿using Newtonsoft.Json;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Globalization;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Helper
{
    public class BillHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IClientService _clientService;
        private readonly IBillService _billService;

        private Bill bill;
        private float moneyValue;
        public BillHandler(IHttpContextAccessor httpContextAccessor, IClientService clientService, IBillService billService)
        {
            _httpContextAccessor = httpContextAccessor;
            _clientService = clientService;
            _billService = billService;
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
            if (bill.optionMoney == 2)
                product.Price = product.Price * GetMoneyValue();
            bool flagProductFound = false;
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

        public void UpdateDiscount(int percent)
        {
            if (bill == null)
                bill = GetBill();
            float percentFloat = (float)percent; // Conversión explícita a flotante

            // Ahora puedes realizar la operación usando percentFloat
            
            bill.PercentDiscount = percent;
            bill.amountDiscount = bill.SubTotal * (percentFloat / 100f);
            updatePriceBill();
            SaveBill();
        }
        public void updatePriceBill()
        {
            //Update bill
            bill.SubTotal = bill.CartXProducts.Sum(x => x.SubTotal);
            bill.TotalCost = bill.SubTotal + bill.delivery.Total - (bill.SubTotal * (bill.PercentDiscount / 100));
            SaveBill();
        }
        //-------------------------------------------------------------------------------------//
        //                           Client Handler                                               //
        //-------------------------------------------------------------------------------------//

        public void updateClientBill(string id)
        {
            if (bill == null)
                bill = GetBill();
            bill.IdClient = id;
            bill.Client = _clientService.GetById(id);
            updatePriceBill();
            SaveBill();
        }

        //-------------------------------------------------------------------------------------//
        //                           Employee Handler                                               //
        //-------------------------------------------------------------------------------------//

        public void AssingEmployee(Employee employee)
        {
            if (bill == null)
                bill = GetBill();
            bill.Employee = employee;
            SaveBill();
        }

        //-------------------------------------------------------------------------------------//
        //                           DElivery Handler                                               //
        //-------------------------------------------------------------------------------------//
        public void UpdateDeliveryBill(bool flag,Delivery delivery)
        {
            if (bill == null)
                bill = GetBill();

            bill.deliveryFlag = flag;
            if (flag)
            {
                //this is beacuse the client should be selected before
                delivery.IdAddress  = bill.Client.IdAddress;

                bill.delivery = delivery;
                bill.delivery.Total = bill.delivery.InternalCost + bill.delivery.deliveryxCompanyTrans.AditionalCompanyCost; //update total delivery
                updatePriceBill();
            }
            else
            {
                bill.delivery = new Delivery();
                updatePriceBill();
            }
            SaveBill();
        }
        //-------------------------------------------------------------------------------------//
        //                           Money Handler                                                //
        //-------------------------------------------------------------------------------------//


        //optionMoney 1 = NIC -> USD

        //optionMoney 2 = USD -> NIC
        public void UpdateMoneyBill(float money, int optionMoney)
        {
            if (bill == null)
                bill = GetBill();
            if(optionMoney == 1)
            {
                foreach (var item in bill.CartXProducts)
                {
                    item.Price = item.Price / money;
                    item.SubTotal = item.Price * item.Quantity;
                }
                bill.SubTotal = bill.CartXProducts.Sum(x => x.SubTotal);
                bill.TotalCost = bill.SubTotal - (bill.SubTotal * (bill.PercentDiscount / 100));
                bill.optionMoney = 1;
            }
            else
            {
                foreach (var item in bill.CartXProducts)
                {
                    item.Price = item.Price * money;
                    item.SubTotal = item.Price * item.Quantity;
                }
                bill.SubTotal = bill.CartXProducts.Sum(x => x.SubTotal);
                bill.TotalCost = bill.SubTotal - (bill.SubTotal * (bill.PercentDiscount / 100));
                bill.optionMoney = 2;
            }
            SaveBill();
        }

        public void UpdateTypePayment(Bill b)
        {
            if (bill == null)
                bill = GetBill();
            bill.IdBusiness = b.IdBusiness;
            bill.bankAccount = b.bankAccount;
            bill.billxTypePaymentxBankAccout = b.billxTypePaymentxBankAccout;
            bill.billxTypePayment = b.billxTypePayment;
            SaveBill();
        }

        //-------------------------------------------------------------------------------------//
        //                           Session Handler                                             //
        //-------------------------------------------------------------------------------------//
        public void ClearData()
        {
            bill = new Bill();
            var sessionData = JsonConvert.SerializeObject(bill);
            _httpContextAccessor.HttpContext.Session.SetString("Bill", sessionData);

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

        public void SaveBill(Bill bill)
        {
            var sessionData = JsonConvert.SerializeObject(bill);
            _httpContextAccessor.HttpContext.Session.SetString("Bill", sessionData);
        }

        public float GetMoneyValue()
        {
            string value = _httpContextAccessor.HttpContext.Session.GetString("MoneyValue");
            return float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            
        }

        public Boolean PurchaseComplete()
        {
            if (bill == null)
                bill = GetBill();

            return _billService.SaveBill(bill);

        }   
    }
}
