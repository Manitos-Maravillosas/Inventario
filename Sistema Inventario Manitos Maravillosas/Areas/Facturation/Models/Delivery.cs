namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models
{
    public class Delivery
    {        
        public int Id { get; set; }
        public float Total { get; set; }
        public float InternalCost { get; set; }
        public string Notes { get; set; }
        public DateOnly DateAprox { get; set; }
        public string Signs { get; set; } 
        public string NameTypeDelivery { get; set; } 
        public int IdBill { get; set; } 
    }
}
