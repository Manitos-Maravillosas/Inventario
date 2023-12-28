namespace Sistema_Inventario_Manitos_Maravillosas.Models.Admin
{
    using System.ComponentModel.DataAnnotations;

    public class Business
    {
        [Key]
        public int IdBusiness { get; set; }

        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Location { get; set; }

        // Navigation properties
        // Add collections for related entities if needed
    }

}
