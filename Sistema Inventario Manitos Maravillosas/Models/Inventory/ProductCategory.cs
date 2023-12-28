using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Models.Inventory
{
    public class ProductCategory
    {
        [Key]
        public int IdProductCategory { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        // Navigation properties
        public ICollection<Product> Products { get; set; }
    }

}
