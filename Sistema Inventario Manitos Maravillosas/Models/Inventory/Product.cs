using Sistema_Inventario_Manitos_Maravillosas.Models.Admin;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Models.Inventory
{
    public class Product
    {
        [Key]
        [StringLength(60)]
        public string IdProduct { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        public float Stock { get; set; }
        public float Cost { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        public bool Status { get; set; }

        // Foreign keys
        public int IdBusiness { get; set; }
        public Business Business { get; set; }

        public int IdProductCategory { get; set; }
        public ProductCategory ProductCategory { get; set; }

        // Navigation properties
        // Add collections for related entities if needed
    }

}
