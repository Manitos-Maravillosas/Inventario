using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.AdminPayment.Models
{
    public class TypePaymentxCoin
    {
        [Required(ErrorMessage = "Se requiere la identificación del tipo de Pago.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Se requiere el nombre del tipo de Pago.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Se requiere la moneda del tipo de Pago.")]
        public string CoinDescription { get; set; }

        public string CoinName { get; set; }
        public int idTypePayment { get; set; }

    }

    public class TypePayment
    {
        public int IdTypePayment { get; set; }
        public string Name { get; set; }
    }

}
