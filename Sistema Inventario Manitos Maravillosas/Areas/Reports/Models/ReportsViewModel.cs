using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Models
{
    [Keyless]
    public class ReportsViewModel
    {
        //Start Date
        [Required]
        public DateTime StartDate { get; set; }
        //End Date
        [Required]
        public DateTime EndDate { get; set; }
        //Report Type
        [Required]
        public string ReportType { get; set; }
        //Report format
        [Required]
        public string ReportFormat { get; set; }
    }
}
