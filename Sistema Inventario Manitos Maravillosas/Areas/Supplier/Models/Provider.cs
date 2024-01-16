using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Supplier.Models
{
    public class Provider
    {
        [Required(ErrorMessage = "Se requiere la identificación del Proveedor.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Se requiere el nombre del proveedor.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Se requiere el número de contacto del Proveedor.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Se requiere la descripción del Proveedor.")]
        public string Description { get; set; }

    }
      
}
