using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;

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
        public int IdDelivery { get; set; }
        public float Total { get; set; }
        public float InternalCost { get; set; }
        public string Notes { get; set; }
        public DateTime DateAprox { get; set; }

        // Foreign keys
        public int IdAddress { get; set; }
        public int IdTypeDelivery { get; set; }
        public int IdBill { get; set; }

        // Navigation properties (optional)
        public Address Address { get; set; }
        public TypeDelivery TypeDelivery { get; set; }
        public Bill Bill { get; set; }
    }
    public class DeliveryxCompanyTrans
    {
        public int IdDeliveryxCompanyTrans { get; set; }
        public float CompanyCost { get; set; }

        // Foreign keys
        public int IdDelivery { get; set; }
        public int IdInChargePaymentDelivery { get; set; }
        public int IdCompanyTrans { get; set; }

        // Navigation properties (optional)
        public Delivery Delivery { get; set; }
        public int InChargePaymentDelivery { get; set; }
        public CompanyTrans CompanyTrans { get; set; }
    }

}
