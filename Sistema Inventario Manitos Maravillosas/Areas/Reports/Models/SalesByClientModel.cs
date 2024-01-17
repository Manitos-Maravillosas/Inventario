namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Models
{
    public class SalesByClientModel
    {
        public int IdClient { get; set; }
        public string Name { get; set; }
        public string LastName1 { get; set; }
        public string LastName2 { get; set; }
        public string PhoneNumber { get; set; }
        public float TotalPurchased { get; set; }
    }
}
