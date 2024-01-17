using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.AdminPayment.Models
{
    public class BankAccount
    {
        [Required(ErrorMessage = "Se requiere la identificación de la cuenta de banco.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Se requiere el número  de la cuenta de banco.")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Se requiere el nombre del banco de la cuenta de banco.")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Se requiere la moneda de la cuenta de banco.")]
        public string CoinDescription { get; set; }

        [Required(ErrorMessage = "Se requiere el tipo de pago de la cuenta de banco.")]
        public string TypePaymentName { get; set; }

    }

}
