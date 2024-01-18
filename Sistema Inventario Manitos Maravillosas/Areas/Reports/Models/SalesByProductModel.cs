namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Models
{
    public class SalesByProductModel
    {
        public string IdProduct { get; set; }
        public string ProductName { get; set; }
        public int AmountSold { get; set; }
        public float TotalCost { get; set; }
        public float TotalSold { get; set; }
        public float TotalProfit { get; set; }
    }
}
