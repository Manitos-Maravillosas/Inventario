namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Models
{
    public class SalesByProductCategoryModel
    {
        public int IdCategory { get; set; }
        public string CategoryName { get; set; }
        public int AmountSold { get; set; }
        public float TotalCost { get; set; }
        public float TotalSold { get; set; }
        public float TotalProfit { get; set; }
    }
}
