using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models
{
    public class TypePayment
    {
        [Required(ErrorMessage = "Se requiere la identificación del tipo de pago.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Se requiere el nombre del tipo de pago.")]
        [StringLength(50, ErrorMessage = "El nombre del cliente debe tener más de 1 letra.", MinimumLength = 2)]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Se requiere el nombre de la moneda")]
        public string CoinName { get; set; }

        [Required(ErrorMessage = "Se requiere la descripción de la moneda")]
        public string CoinDescription { get; set; }

    }
}
