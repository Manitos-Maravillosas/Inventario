using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models
{
    public class TypeDelivery
    {
        [Required(ErrorMessage = "Se requiere la identificación del tipo de Envío.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Se requiere el nombre del tipo de Envío.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Se requiere la descripción del tipo de Envío.")]
        public string Description { get; set; }
        
    }
      
}
