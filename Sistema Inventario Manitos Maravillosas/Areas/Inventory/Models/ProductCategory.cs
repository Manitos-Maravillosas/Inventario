using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Inventory.Models
{
    public class ProductCategory
    {
        [Key]
        public int IdProductCategory { get; set; }

        [Required(ErrorMessage = "Se requiere el nombre de la categoría.")]
        [StringLength(50, ErrorMessage = "El nombre de la categoría debe estar entre 2 y 50 caracteres", MinimumLength = 2)]
        public string Category { get; set; }

        [Required(ErrorMessage = "Se requiere el nombre de la categoría.")]
        [StringLength(50, ErrorMessage = "El nombre de la categoría debe estar entre 2 y 50 caracteres", MinimumLength = 2)]
        public string Description { get; set; }
    }

}
