using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models
{
    public class TypePayment
    {        
        public int Id { get; set; }
        public string Name { get; set; }    
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
