namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Models
{
    public class SalesByBusinessModel
    {
        public int IdBusiness { get; set; }
        public string BusinessName { get; set; }
        public float TotalCost { get; set; }
        public float TotalSold { get; set; }
        public float TotalProfit { get; set; }
    }
}
