using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Supplier.Models
{
    public class PurchaseProvider
    {
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
    }
}
