using Microsoft.EntityFrameworkCore;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Models
{
    [Keyless]
    public class ReportsViewModel
    {
        //Start Date
        public string StartDate { get; set; }
        //End Date
        public string EndDate { get; set; }
        //Report Type
        public string ReportType { get; set; }
    }
}
