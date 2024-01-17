using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models
{
    public class TypePayment
    {
        [Required(ErrorMessage = "Se requiere la identificación del tipo de Pago.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Se requiere el nombre del tipo de Pago.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Se requiere la moneda del tipo de Pago.")]
        public string CoinDescription { get; set; }
        
    }

    public class TypePaymentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CoinDescription { get; set; }
        public string CoinName { get; set; } 
    }

}
