using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.AdminPayment.Models
{
    public class BankAccount
    {
        [Required(ErrorMessage = "Se requiere la identificación de la cuenta de banco.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Se requiere el número  de la cuenta de banco.")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Se requiere el una moneda.")]
        public int IdCoin { get; set; }

        [Required(ErrorMessage = "Se requiere seleccione una entidad bancaria.")]
        public int IdBank { get; set; }

        [Required(ErrorMessage = "Se requiere seleccionar un tipo de pago.")]
        public int IdTypePayment { get; set; }

        

        public string? CoinName { get; set; }        

        public int idTypePaymentxCoin { get; set; }
  
        public string? TypePaymentName { get; set; }
        public string? CoinDescription { get; set; }
        public string? BankName { get; set; }

    }

    public class Bank
    {
        public int IdBank { get; set; }
        public string Name { get; set; }

        public string Icon { get; set; }

        public string Description { get; set; }
    }

}
