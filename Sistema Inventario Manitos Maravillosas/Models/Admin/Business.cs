namespace Sistema_Inventario_Manitos_Maravillosas.Models.Admin
{
    using System.ComponentModel.DataAnnotations;

    public class Business
    {
        [Key]
        [Required(ErrorMessage = "Se requiere el ID del negocio.")]
        public int IdBusiness { get; set; }

        [Required(ErrorMessage = "Se requiere el nombre del negocio.")]
        [StringLength(60, ErrorMessage = "El nombre del negocio debe tener más de 1 letra.")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "La descripción del negocio debe tener más de 1 letra.")]
        public string Description { get; set; }

        [StringLength(50, ErrorMessage = "La ubicación del negocio debe tener más de 1 letra.")]
        public string Location { get; set; }
    }

}
