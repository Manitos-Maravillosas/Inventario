namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Models
{
    public class BillsByClientModel
    {
        public int IdBill { get; set; }
        public DateTime Date { get; set; }
        public float PercentDiscount { get; set; }
        public float SubTotal { get; set; }
        public float TotalCost { get; set; }
        public string BusinessName { get; set; }
    }
}
