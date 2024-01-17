using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class HomeController : Controller
    {
        private readonly IReportsService _reportsService;

        public HomeController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Report(ReportsViewModel reportsView)
        {
            if (ModelState.IsValid)
            {
                //TODO: Generate report

            }
            return RedirectToAction("Index");
        }

        public IActionResult ExportReport(ReportsViewModel reportsView)
        {
            // switch to check for format
            if (reportsView.ReportFormat == "pdf")
            {
                return ExportPDF(reportsView);
            }
            else
            {
                return ExportExcel(reportsView);
            }
        }

        private IActionResult ExportExcel(ReportsViewModel reportsView)
        {
            throw new NotImplementedException();
        }

        // PDF Generation

        public Document GeneratePDFLayout(string reportType, MemoryStream stream)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(stream));
            Document doc = new Document(pdfDoc);

            // Bold style
            Style boldStyle = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetFontSize(10);


            doc.Add(new Paragraph("MANITOS MARAVILLOSAS")
                            .AddStyle(boldStyle)
                            .SetTextAlignment(TextAlignment.CENTER));

            doc.Add(new Paragraph(reportType)
                            .AddStyle(boldStyle)
                            .SetTextAlignment(TextAlignment.CENTER));

            return doc;
        }

        public IActionResult ExportPDF(ReportsViewModel reportsView)
        {
            // Memory stream to store the PDF
            MemoryStream stream;
            // switch to check for type of report
            switch (reportsView.ReportType)
            {
                case "totalSales":
                    stream = ExportTotalSalesPDF(reportsView);
                    break;
                case "totalPurchases":
                    stream = ExportTotalPurchasesPDF(reportsView);
                    break;
                case "byProduct":
                    stream = ExportTotalProductsPDF(reportsView);
                    break;
                case "byCategory":
                    stream = ExportTotalCategoriesPDF(reportsView);
                    break;
                case "byBusiness":
                    stream = ExportTotalBusinessPDF(reportsView);
                    break;
                default:
                    stream = null;
                    break;
            }

            // Return the file
            var content = stream.ToArray();
            return File(content, "application/pdf", "Reporte.pdf");
        }

        private MemoryStream ExportTotalSalesPDF(ReportsViewModel reportsView)
        {
            // Memory stream to store the PDF
            MemoryStream stream = new MemoryStream();

            // Generate the PDF layout
            Document doc = GeneratePDFLayout("Reporte de ventas", stream);

            // Add the table
            Table table = new Table(7, false);
            table.SetWidth(UnitValue.CreatePercentValue(100));

            // Add the headers
            table.AddHeaderCell("Id Factura");
            table.AddHeaderCell("Fecha");
            table.AddHeaderCell("Descuento");
            table.AddHeaderCell("Subtotal");
            table.AddHeaderCell("Total");
            table.AddHeaderCell("Empleado");
            table.AddHeaderCell("Cliente");
            table.AddHeaderCell("Negocio");

            // Add the data
            foreach (TotalSalesModel sale in _reportsService.GetTotalSales(reportsView.StartDate, reportsView.EndDate))
            {
                table.AddCell(sale.IdBill.ToString());
                table.AddCell(sale.Date.ToString());
                table.AddCell(sale.PercentDiscount.ToString());
                table.AddCell(sale.SubTotal.ToString());
                table.AddCell(sale.TotalCost.ToString());
                table.AddCell(sale.EmployeeName);
                table.AddCell(sale.ClientName);
                table.AddCell(sale.BusinessName);
            }

            // Add the table to the document
            doc.Add(table);

            // Close the document
            doc.Close();

            // Return the stream
            return new MemoryStream(stream.ToArray());
        }

        private MemoryStream ExportTotalPurchasesPDF(ReportsViewModel reportsView)
        {
            throw new NotImplementedException();
        }

        private MemoryStream ExportTotalProductsPDF(ReportsViewModel reportsView)
        {
            throw new NotImplementedException();
        }

        private MemoryStream ExportTotalCategoriesPDF(ReportsViewModel reportsView)
        {
            throw new NotImplementedException();
        }

        private MemoryStream ExportTotalBusinessPDF(ReportsViewModel reportsView)
        {
            throw new NotImplementedException();
        }
    }
}

