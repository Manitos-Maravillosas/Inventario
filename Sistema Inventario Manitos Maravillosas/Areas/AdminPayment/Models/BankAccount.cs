using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.AdminPayment.Models
{
    public class BankAccount
    {
        [Required(ErrorMessage = "Se requiere la identificación de la cuenta de banco.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Se requiere el número  de la cuenta de banco.")]
        public string AccountNumber { get; set; }

        public int IdBank { get; set; }

        [Required(ErrorMessage = "Se requiere el nombre del banco de la cuenta de banco.")]
        public string BankName { get; set; }

        public int IdCoin { get; set; }
        public int CoinName { get; set; }


        [Required(ErrorMessage = "Se requiere la moneda de la cuenta de banco.")]
        public string CoinDescription { get; set; }

        public int idTypePaymentxCoin { get; set; }
        public int IdTypePayment { get; set; }

        [Required(ErrorMessage = "Se requiere el tipo de pago de la cuenta de banco.")]
        public string TypePaymentName { get; set; }

    }

    public class Bank
    {
        public int IdBank { get; set; }
        public string Name { get; set; }

        public string Icon { get; set; }

        public string Description { get; set; }
    }

}
