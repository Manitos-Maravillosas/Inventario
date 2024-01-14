using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Inventory.Models
{
    public class Product
    {
        [Key]
        [Required(ErrorMessage = "Se requiere el ID del producto.")]
        [StringLength(60, ErrorMessage = "El ID del producto debe estar entre 2 y 60 caracteres", MinimumLength = 2)]
        public string IdProduct { get; set; }

        [Required(ErrorMessage = "Se requiere el nombre del producto.")]
        [StringLength(60, ErrorMessage = "El nombre del producto debe estar entre 2 y 60 caracteres", MinimumLength = 2)]
        public string ProductName { get; set; }

        [Range(0, 1000000, ErrorMessage = "El stock debe ser mayor o igual a 0.")]
        public float Stock { get; set; } = 0;

        [Range(0, 1000000, ErrorMessage = "El costo debe ser mayor o igual a 0.")]
        public float Cost { get; set; } = 0;

        [Range(0, 1000000, ErrorMessage = "El precio debe ser mayor o igual a 0.")]
        public float Price { get; set; } = 0;

        [Required(ErrorMessage = "Se requiere la descripción del producto.")]
        [StringLength(150, ErrorMessage = "La descripción del producto debe estar entre 2 y 150 caracteres", MinimumLength = 2)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Se requiere el estado del producto.")]
        public bool Status { get; set; }

        [Required(ErrorMessage = "Se requiere la categoría del producto.")]
        public string ProductCategory { get; set; }

        [Required(ErrorMessage = "Se requiere el negocio al que pertenece el producto.")]
        public string BusinessName { get; set; }
    }
}
