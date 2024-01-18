using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.AdminPayment.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models.Admin;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models
{
    public class Bill
    {
        public Bill()
        {
            // Set default values in the constructor
            Date = DateTime.Now;
            PercentDiscount = 0.0f;
            SubTotal = 0.0f;
            TotalCost = 0.0f;
            IdClient = "defaultClientId";     // Set a default or fetch from user context
            IdBusiness = 1;                   // Set a default business ID
            CartXProducts = new List<CartXProduct>();
            Products = new List<ProductFacturation>();
            optionMoney = 1;
            deliveryFlag = false;
            delivery = new Delivery();
            bankAccount = new BankAccount();
            billxTypePayment = new BillxTypePayment();
            billxTypePaymentxBankAccout = new BillxTypePaymentxBankAccout();

        }

    

        [Key]
        public int IdBill { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public float PercentDiscount { get; set; }

        public float amountDiscount { get; set;} 

        public float SubTotal { get; set; }

        public float TotalCost { get; set; }

        public string? IdEmployee { get; set; }
        public virtual Employee Employee { get; set; }

        public string IdClient { get; set; }
        public virtual Client Client { get; set; }

        public int IdBusiness { get; set; }

        public string? BusinessName { get; set; }

        // Navigation property for CartXProduct
        public virtual List<CartXProduct> CartXProducts { get; set; }

        public List<ProductFacturation> Products { get; set; }


        public int optionMoney { get; set; }

        public List<Client> listClients { get; set; }

       //delivery
       public bool deliveryFlag { get; set; }

        public int idDelivery { get; set; }
        public Delivery delivery { get; set; }

        public BankAccount bankAccount { get; set; }

        public BillxTypePayment billxTypePayment { get; set; }

        public BillxTypePaymentxBankAccout billxTypePaymentxBankAccout { get; set; }

        public bool mixPayment { get; set; }
    }

    public class BillxTypePaymentxBankAccout
    {
        public int IdBillxTypePaymentxBankAccout { get; set; }
        public int idBillxIdTypePayment { get; set; }
        public int idBankAccount { get; set; }
    }

    public class CartXProduct
    {
        public CartXProduct()
        {
            // Set default values in the constructor
            Quantity = 0;
            Cost = 0.0f;
            Price = 0.0f;
            SubTotal = 0.0f;
            Product = new ProductFacturation();  // IdProduct should be set explicitly as it is a foreign key
            // IdProduct and IdBill should be set explicitly as they are foreign keys
        }
        [Key]
        public int IdCartXProduct { get; set; }

        public int Quantity { get; set; }

        public float Cost { get; set; }

        public float Price { get; set; }

        public float SubTotal { get; set; }

        // Foreign keys
        public string IdProduct { get; set; }
        public virtual ProductFacturation Product { get; set; }

        public int IdBill { get; set; }
        public virtual Bill Bill { get; set; }


    }

    public class BillxTypePayment
    {
        public BillxTypePayment()
        {
            amountPaid = 0.0f;
            idTypePaymentxCoin = 0;
            typePaymentxCoin = new TypePaymentxCoin();       
            
            bothCoins = false;
            amountPaidDolar = 0.0f;
            amountPaidCordoba = 0.0f;

        }
        [Key]
        public bool bothCoins { get; set; }
        public float amountPaidDolar { get; set; }
        public float amountPaidCordoba { get; set; }


        // Foreign keys
        public float amountPaid { get; set; }
        public int idTypePaymentxCoin { get; set; }
        public virtual TypePaymentxCoin typePaymentxCoin { get; set; }
    }
}
