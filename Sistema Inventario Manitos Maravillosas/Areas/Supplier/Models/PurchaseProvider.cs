using Microsoft.Extensions.Hosting;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Inventory.Models;
using System.ComponentModel.DataAnnotations;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using System.Configuration.Provider;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models.Admin;
using System.Net.Sockets;


namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Supplier.Models
{        
    public class PurchaseProvider
    {
        public PurchaseProvider()
        {
            Date = DateTime.Now;
            Cant = 0;
            Cost = 0;
            Total = 0;
            ProductName = string.Empty;
            ProviderName = string.Empty;
            BusinessName = string.Empty;
            Products = new List<Product>(); 
            Price = 0;
        }
        [Required(ErrorMessage = "Se requiere la identificación de la compra.")]
        public int Id{ get; set; }

        [Required(ErrorMessage = "Se requiere la fecha de la compra.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Se requiere la cantidad de la compra.")]
        public int Cant { get; set; }

        [Required(ErrorMessage = "Se requiere el costo de la compra.")]
        public float Cost { get; set; }

        [Required(ErrorMessage = "Se requiere el total de la compra.")]
        public float Total { get; set; }

        [Required(ErrorMessage = "Se requiere el nombre del producto de la compra.")]
        public string ProductName { get; set; } 

        [Required(ErrorMessage = "Se requiere el nombre del proveedor de la compra.")]
        public string ProviderName { get; set; }

        [Required(ErrorMessage = "Se requiere el nombre del negocio que hace la compra.")]
        public string BusinessName { get; set; }
        public IEnumerable<Product> Products { get; set; }

        public float Price { get; set; }

    }

}
