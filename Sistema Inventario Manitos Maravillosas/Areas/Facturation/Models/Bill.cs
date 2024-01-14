using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models.Admin;
using System.ComponentModel.DataAnnotations;

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
            IdEmployee = "defaultEmployeeId"; // Set a default or fetch from user context
            IdClient = "defaultClientId";     // Set a default or fetch from user context
            IdBusiness = 1;                   // Set a default business ID
            CartXProducts = new List<CartXProduct>();
            Products = new List<Product>();
        }

        [Key]
        public int IdBill { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public float PercentDiscount { get; set; }

        public float SubTotal { get; set; }

        public float TotalCost { get; set; }

        // Foreign keys
        public string IdEmployee { get; set; }
        public virtual Employee Employee { get; set; }

        public string IdClient { get; set; }
        public virtual Client Client { get; set; }

        public int IdBusiness { get; set; }
        public virtual Business Business { get; set; }

        // Navigation property for CartXProduct
        public virtual ICollection<CartXProduct> CartXProducts { get; set; }

        public List<Product> Products { get; set; }

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
        public virtual Product Product { get; set; }

        public int IdBill { get; set; }
        public virtual Bill Bill { get; set; }
    }
}
