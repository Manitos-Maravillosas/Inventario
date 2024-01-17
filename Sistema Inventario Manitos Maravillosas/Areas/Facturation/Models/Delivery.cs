using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Inventory.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models.Admin;
using System.Net.Sockets;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models
{
    public class CompanyTrans
    {
        public int IdCompanyTrans { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }


    public class Delivery
    {
        public Delivery()
        {
            Total = 0.0f;
            InternalCost = 0.0f;
            deliveryxCompanyTrans = new DeliveryxCompanyTrans();
            DateAprox = DateTime.Now;

        }
        public int Id { get; set; }
        public float Total { get; set; }
        public float InternalCost { get; set; }
        public string Notes { get; set; }
        [BindProperty]
        public DateTime DateAprox { get; set; }
        public string Signs { get; set; }
        public string NameTypeDelivery { get; set; }

        // Foreign keys
        public int IdAddress { get; set; }
        public int IdTypeDelivery { get; set; }
        public int IdBill { get; set; }

        // Navigation properties (optional)
        public Address Address { get; set; }
        public TypeDelivery TypeDelivery { get; set; }
        public Bill Bill { get; set; }

        //public
        public DeliveryxCompanyTrans deliveryxCompanyTrans { get; set;}         
    }
    public class DeliveryxCompanyTrans
    {
        public DeliveryxCompanyTrans()
        {
            AditionalCompanyCost = 0.0f;
            InChargePaymentDelivery = "2";
        }
        public float AditionalCompanyCost { get; set; }
        public int IdCompanyTrans { get; set; }
        [BindProperty]
        public string InChargePaymentDelivery { get; set; }
        public string[] InChargePaymentDeliveries = new[] { "1", "2"};
    }

}
