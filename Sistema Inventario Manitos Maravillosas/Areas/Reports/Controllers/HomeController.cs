using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Controllers
{
    [Area("Reports")]
    [Authorize(Roles = "Administrador")]
    public class HomeController : Controller
    {
        private readonly IReportsService _reportsService;

        public HomeController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        public IActionResult Index()
        {
            // Get the start of month
            var startDate = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);
            // Get the end of month
            var endDate = startDate.AddMonths(1).AddDays(-1);
            // Get the financial summary from the start of month to end of month
            FinancialSummary financialSummary = _reportsService.GetFinancialSummary(startDate, endDate);

            ViewData["FinancialSummary"] = financialSummary;
            ReportsViewModel reportsView = new ReportsViewModel();
            reportsView.StartDate = startDate;
            reportsView.EndDate = endDate;
            return View(reportsView);
        }

        // GET - Chart
        public object GetDataFromService()
        {
            // Get the start of month
            var startDate = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);
            // Get the end of month
            var endDate = startDate.AddMonths(1).AddDays(-1);

            // Get the sales by business
            var salesByBusiness = _reportsService.GetSalesByBusiness(startDate, endDate);

            //Check if salesByBusiness count is 0
            if (salesByBusiness.Count == 0)
            {
                return new
                {
                    ManitosMaravillosas = 0,
                    DonMae = 0
                };
            }

            // Get the profit of Manitos Maravillosas
            var profitManitosMaravillosas = salesByBusiness.Where(s => s.IdBusiness == 1).FirstOrDefault() == null ? 0 : salesByBusiness.Where(s => s.IdBusiness == 1).FirstOrDefault().TotalProfit;


            // Get the profit of Don Mae
            var profitDonMae = salesByBusiness.Where(s => s.IdBusiness == 2).FirstOrDefault() == null ? 0 : salesByBusiness.Where(s => s.IdBusiness == 2).FirstOrDefault().TotalProfit;

            return new
            {
                ManitosMaravillosas = profitManitosMaravillosas,
                DonMae = profitDonMae
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Report(ReportsViewModel reportsView)
        {
            if (ModelState.IsValid)
            {
                //TODO: Generate report
                return RedirectToAction("ExportReport", reportsView);

            }
            TempData["ErrorMessage"] = "El formato o rango de fechas no es valido";
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

        public Document GeneratePDFLayout(string reportType, MemoryStream stream, DateTime startDate, DateTime endDate)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(stream));
            Document doc = new Document(pdfDoc);

            // Set document font size to 10
            doc.SetFontSize(10);

            // Bold style
            Style boldStyle = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD));

            // Title and subtitle style
            Style titleStyle = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetFontSize(16);

            Style subtitleStyle = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetFontSize(14);



            doc.Add(new Paragraph("MANITOS MARAVILLOSAS")
                            .AddStyle(titleStyle)
                            .SetTextAlignment(TextAlignment.CENTER));

            doc.Add(new Paragraph(reportType)
                            .AddStyle(subtitleStyle)
                            .SetTextAlignment(TextAlignment.CENTER));

            doc.Add(new Paragraph()
                .Add(new Text("Fecha de inicio: ").AddStyle(boldStyle)).Add($"{startDate.ToString("dd/MM/yyyy")}")
                .Add(new Text("\nFecha de fin: ").AddStyle(boldStyle)).Add($"{endDate.ToString("dd/MM/yyyy")}")
                .Add(new Text("\nFecha de generación: ").AddStyle(boldStyle)).Add($"{DateTime.Now.ToString("dd/MM/yyyy")}\n\n")
                );

            return doc;
        }

        public IActionResult ExportPDF(ReportsViewModel reportsView)
        {
            // Memory stream to store the PDF
            MemoryStream stream;
            // Filename
            string fileName = "";
            // switch to check for type of report
            switch (reportsView.ReportType)
            {
                case "totalSales":
                    stream = ExportTotalSalesPDF(reportsView);
                    fileName = "Reporte de ventas totales.pdf";
                    break;
                case "totalPurchases":
                    stream = ExportTotalPurchasesPDF(reportsView);
                    fileName = "Reporte de compras totales.pdf";
                    break;
                case "byProduct":
                    stream = ExportTotalProductsPDF(reportsView);
                    fileName = "Reporte de ventas por producto.pdf";
                    break;
                case "byCategory":
                    stream = ExportTotalCategoriesPDF(reportsView);
                    fileName = "Reporte de ventas por categoria.pdf";
                    break;
                case "byClient":
                    stream = ExportTotalClientsPDF(reportsView);
                    fileName = "Reporte de ventas por cliente.pdf";
                    break;
                case "byBusiness":
                    stream = ExportTotalBusinessPDF(reportsView);
                    fileName = "Reporte de ventas por negocio.pdf";
                    break;
                default:
                    stream = null;
                    break;
            }

            // Return the file
            var content = stream.ToArray();
            return File(content, "application/pdf", fileName);
        }

        private MemoryStream ExportTotalSalesPDF(ReportsViewModel reportsView)
        {
            // Memory stream to store the PDF
            MemoryStream stream = new MemoryStream();

            // Generate the PDF layout
            Document doc = GeneratePDFLayout("Reporte de ventas totales", stream, reportsView.StartDate, reportsView.EndDate);

            // Style for the table headers
            Style headerStyle = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetFontSize(10);

            // Font size 8 for the table data
            Style tableDataStyle = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(8);

            // Create style bg color light gray
            Style lightGrayStyle = new Style()
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY);

            // Add the table
            Table table = new Table(8, false);
            table.SetWidth(UnitValue.CreatePercentValue(100));

            // Add the headers with style
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Id Factura").AddStyle(headerStyle)).AddStyle(lightGrayStyle));
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Fecha").AddStyle(headerStyle)).AddStyle(lightGrayStyle));
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Descuento (%)").AddStyle(headerStyle)).AddStyle(lightGrayStyle));
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Subtotal ($)").AddStyle(headerStyle)).AddStyle(lightGrayStyle));
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Total ($)").AddStyle(headerStyle)).AddStyle(lightGrayStyle));
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Empleado").AddStyle(headerStyle)).AddStyle(lightGrayStyle));
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Cliente").AddStyle(headerStyle)).AddStyle(lightGrayStyle));
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Negocio").AddStyle(headerStyle)).AddStyle(lightGrayStyle));

            table.AddStyle(tableDataStyle);

            int cont = 1;

            // Add the data
            foreach (TotalSalesModel sale in _reportsService.GetTotalSales(reportsView.StartDate, reportsView.EndDate))
            {
                //Every other row is gray
                if (cont % 2 == 0)
                {
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.IdBill.ToString())).AddStyle(lightGrayStyle));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.Date.ToString(("dd/MM/yyyy")))).AddStyle(lightGrayStyle));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.PercentDiscount.ToString())).AddStyle(lightGrayStyle));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.SubTotal.ToString())).AddStyle(lightGrayStyle));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.TotalCost.ToString())).AddStyle(lightGrayStyle));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.EmployeeName)).AddStyle(lightGrayStyle));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.ClientName)).AddStyle(lightGrayStyle));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.BusinessName)).AddStyle(lightGrayStyle));
                }
                else
                {
                    table.AddCell(sale.IdBill.ToString());
                    table.AddCell(sale.Date.ToString(("dd/MM/yyyy")));
                    table.AddCell(sale.PercentDiscount.ToString());
                    table.AddCell(sale.SubTotal.ToString());
                    table.AddCell(sale.TotalCost.ToString());
                    table.AddCell(sale.EmployeeName);
                    table.AddCell(sale.ClientName);
                    table.AddCell(sale.BusinessName);
                }
                cont++;
            }

            // Add the table to the document
            doc.Add(table);

            // Close the document
            doc.Close();

            // Return the stream
            return new MemoryStream(stream.ToArray());
        }

        private MemoryStream ExportTotalProductsPDF(ReportsViewModel reportsView)
        {
            // Memory stream to store the PDF
            MemoryStream stream = new MemoryStream();

            // Generate the PDF layout
            Document doc = GeneratePDFLayout("Reporte de ventas por producto", stream, reportsView.StartDate, reportsView.EndDate);

            // Style for the table headers
            Style headerStyle = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetFontSize(10);

            // Font size 8 for the table data
            Style tableDataStyle = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(8);

            // Create style bg color light gray
            Style lightGrayStyle = new Style()
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY);


            // Add the table
            Table table = new Table(6, false);
            table.SetWidth(UnitValue.CreatePercentValue(100));

            // Add the headers with style
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Id Producto").AddStyle(headerStyle)).AddStyle(lightGrayStyle));
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Nombre").AddStyle(headerStyle)).AddStyle(lightGrayStyle));
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Cantidad Vendida").AddStyle(headerStyle)).AddStyle(lightGrayStyle));
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Costo Total ($)").AddStyle(headerStyle)).AddStyle(lightGrayStyle));
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Total Vendido ($)").AddStyle(headerStyle)).AddStyle(lightGrayStyle));
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Ganancia Total ($)").AddStyle(headerStyle)).AddStyle(lightGrayStyle));

            table.AddStyle(tableDataStyle);

            int cont = 1;

            // Add the data
            foreach (SalesByProductModel sale in _reportsService.GetSalesByProduct(reportsView.StartDate, reportsView.EndDate))
            {
                //Every other row is gray
                if (cont % 2 == 0)
                {
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.IdProduct.ToString())).AddStyle(lightGrayStyle));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.ProductName)).AddStyle(lightGrayStyle));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.AmountSold.ToString())).AddStyle(lightGrayStyle));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.TotalCost.ToString())).AddStyle(lightGrayStyle));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.TotalSold.ToString())).AddStyle(lightGrayStyle));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.TotalProfit.ToString())).AddStyle(lightGrayStyle));
                }
                else
                {
                    table.AddCell(sale.IdProduct.ToString());
                    table.AddCell(sale.ProductName);
                    table.AddCell(sale.AmountSold.ToString());
                    table.AddCell(sale.TotalCost.ToString());
                    table.AddCell(sale.TotalSold.ToString());
                    table.AddCell(sale.TotalProfit.ToString());
                }
                cont++;
            }

            // Add the table to the document
            doc.Add(table);

            // Close the document
            doc.Close();

            // Return the stream
            return new MemoryStream(stream.ToArray());
        }

        private MemoryStream ExportTotalCategoriesPDF(ReportsViewModel reportsView)
        {
            // Memory stream to store the PDF
            MemoryStream stream = new MemoryStream();

            // Generate the PDF layout
            Document doc = GeneratePDFLayout("Reporte de ventas por categoria", stream, reportsView.StartDate, reportsView.EndDate);

            // Style for the table headers
            Style headerStyle = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetFontSize(10);

            // Font size 8 for the table data
            Style tableDataStyle = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(8);

            // Create style bg color light gray
            Style lightGrayStyle = new Style()
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY);


            // Add the table
            Table table = new Table(6, false);
            table.SetWidth(UnitValue.CreatePercentValue(100));

            // Add the headers with style
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Id Categoria").AddStyle(headerStyle)).AddStyle(lightGrayStyle));
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Nombre").AddStyle(headerStyle)).AddStyle(lightGrayStyle));
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Cantidad Vendida").AddStyle(headerStyle)).AddStyle(lightGrayStyle));
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Costo Total ($)").AddStyle(headerStyle)).AddStyle(lightGrayStyle));
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Total Vendido ($)").AddStyle(headerStyle)).AddStyle(lightGrayStyle));
            table.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Ganancia Total ($)").AddStyle(headerStyle)).AddStyle(lightGrayStyle));

            table.AddStyle(tableDataStyle);

            int cont = 1;

            // Add the data
            foreach (SalesByProductCategoryModel sale in _reportsService.GetSalesByProductCategory(reportsView.StartDate, reportsView.EndDate))
            {
                //Every other row is gray
                if (cont % 2 == 0)
                {
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.IdCategory.ToString())).AddStyle(lightGrayStyle));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.CategoryName)).AddStyle(lightGrayStyle));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.AmountSold.ToString())).AddStyle(lightGrayStyle));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.TotalCost.ToString())).AddStyle(lightGrayStyle));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.TotalSold.ToString())).AddStyle(lightGrayStyle));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(sale.TotalProfit.ToString())).AddStyle(lightGrayStyle));
                }
                else
                {
                    table.AddCell(sale.IdCategory.ToString());
                    table.AddCell(sale.CategoryName);
                    table.AddCell(sale.AmountSold.ToString());
                    table.AddCell(sale.TotalCost.ToString());
                    table.AddCell(sale.TotalSold.ToString());
                    table.AddCell(sale.TotalProfit.ToString());
                }
                cont++;
            }

            // Add the table to the document
            doc.Add(table);

            // Close the document
            doc.Close();

            // Return the stream
            return new MemoryStream(stream.ToArray());
        }

        private MemoryStream ExportTotalClientsPDF(ReportsViewModel reportsView)
        {
            // Memory stream to store the PDF
            MemoryStream stream = new MemoryStream();

            // Generate the PDF layout
            Document doc = GeneratePDFLayout("Reporte de ventas por cliente", stream, reportsView.StartDate, reportsView.EndDate);

            // bold style
            Style boldStyle = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD));

            // Style for the table headers
            Style headerStyleTable = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetFontSize(10);

            // Font size 8 for the table data
            Style tableDataStyle = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(8);

            // Create style bg color light gray
            Style lightGrayStyle = new Style()
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY);

            // Create style for header
            Style header1Style = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetFontSize(14);

            Style header2Style = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetFontSize(12);

            List<SalesByClientModel> salesByClient = _reportsService.GetSalesByClient(reportsView.StartDate, reportsView.EndDate);

            // Loop through the clients sales by clients
            foreach (SalesByClientModel sale in salesByClient)
            {
                // Add the client name header1 style
                doc.Add(new Paragraph($"{sale.Name} {sale.LastName1} {sale.LastName2}").AddStyle(header1Style));

                // Add the client phone number and total purchased bold for tag
                doc.Add(new Paragraph()
                    .Add(new Text("Teléfono: ").AddStyle(boldStyle)).Add($"{sale.PhoneNumber}")
                    .Add(new Text("\nTotal comprado:").AddStyle(boldStyle)).Add($"${sale.TotalPurchased}")
                );
                // Add the header for the table
                doc.Add(new Paragraph("Facturas").AddStyle(header2Style));

                // Add the table
                Table tableBillsByClient = new Table(6, false);
                tableBillsByClient.SetWidth(UnitValue.CreatePercentValue(100));

                // Add the headers with style
                tableBillsByClient.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Id Factura").AddStyle(headerStyleTable)).AddStyle(lightGrayStyle));
                tableBillsByClient.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Fecha").AddStyle(headerStyleTable)).AddStyle(lightGrayStyle));
                tableBillsByClient.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Descuento (%)").AddStyle(headerStyleTable)).AddStyle(lightGrayStyle));
                tableBillsByClient.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Subtotal ($)").AddStyle(headerStyleTable)).AddStyle(lightGrayStyle));
                tableBillsByClient.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Total ($)").AddStyle(headerStyleTable)).AddStyle(lightGrayStyle));
                tableBillsByClient.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Negocio").AddStyle(headerStyleTable)).AddStyle(lightGrayStyle));

                tableBillsByClient.AddStyle(tableDataStyle);

                int cont = 1;

                // Add the data
                foreach (BillsByClientModel bill in _reportsService.GetBillsByClient(reportsView.StartDate, reportsView.EndDate, sale.IdClient))
                {
                    //Every other row is gray
                    if (cont % 2 == 0)
                    {
                        tableBillsByClient.AddCell(new Cell(1, 1).Add(new Paragraph(bill.IdBill.ToString())).AddStyle(lightGrayStyle));
                        tableBillsByClient.AddCell(new Cell(1, 1).Add(new Paragraph(bill.Date.ToString(("dd/MM/yyyy")))).AddStyle(lightGrayStyle));
                        tableBillsByClient.AddCell(new Cell(1, 1).Add(new Paragraph(bill.PercentDiscount.ToString())).AddStyle(lightGrayStyle));
                        tableBillsByClient.AddCell(new Cell(1, 1).Add(new Paragraph(bill.SubTotal.ToString())).AddStyle(lightGrayStyle));
                        tableBillsByClient.AddCell(new Cell(1, 1).Add(new Paragraph(bill.TotalCost.ToString())).AddStyle(lightGrayStyle));
                        tableBillsByClient.AddCell(new Cell(1, 1).Add(new Paragraph(bill.BusinessName)).AddStyle(lightGrayStyle));
                    }
                    else
                    {
                        tableBillsByClient.AddCell(bill.IdBill.ToString());
                        tableBillsByClient.AddCell(bill.Date.ToString(("dd/MM/yyyy")));
                        tableBillsByClient.AddCell(bill.PercentDiscount.ToString());
                        tableBillsByClient.AddCell(bill.SubTotal.ToString());
                        tableBillsByClient.AddCell(bill.TotalCost.ToString());
                        tableBillsByClient.AddCell(bill.BusinessName);
                    }
                    cont++;
                }
                // Add the table to the document
                doc.Add(tableBillsByClient);

                // Add a page break if there are more clients
                if (sale != salesByClient.Last())
                {
                    doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                }
            }

            // Close the document
            doc.Close();

            // Return the stream
            return new MemoryStream(stream.ToArray());
        }

        private MemoryStream ExportTotalBusinessPDF(ReportsViewModel reportsView)
        {
            // Memory stream to store the PDF
            MemoryStream stream = new MemoryStream();

            // Generate the PDF layout
            Document doc = GeneratePDFLayout("Reporte de ventas por negocio", stream, reportsView.StartDate, reportsView.EndDate);

            // bold style
            Style boldStyle = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD));

            // Style for the table headers
            Style headerStyleTable = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetFontSize(10);

            // Font size 8 for the table data
            Style tableDataStyle = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(8);

            // Create style bg color light gray
            Style lightGrayStyle = new Style()
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY);

            // Create style for header
            Style header1Style = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetFontSize(14);

            Style header2Style = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetFontSize(12);

            List<SalesByBusinessModel> salesByBusiness = _reportsService.GetSalesByBusiness(reportsView.StartDate, reportsView.EndDate);

            // Loop through the clients sales by clients
            foreach (SalesByBusinessModel sale in salesByBusiness)
            {
                // Add the client name header1 style
                doc.Add(new Paragraph($"{sale.BusinessName}").AddStyle(header1Style));

                // Add the totalCost, totalSold and totalProfit bold for tag
                doc.Add(new Paragraph()
                    .Add(new Text("Total gastado: ").AddStyle(boldStyle)).Add($"${sale.TotalCost}")
                    .Add(new Text("\nTotal vendido: ").AddStyle(boldStyle)).Add($"${sale.TotalSold}")
                    .Add(new Text("\nGanancia total: ").AddStyle(boldStyle)).Add($"${sale.TotalProfit}\n\n")
                );
                // Add the header for the table
                doc.Add(new Paragraph("Facturas").AddStyle(header2Style));

                // Add the table
                Table tableBillsByBusiness = new Table(7, false);
                tableBillsByBusiness.SetWidth(UnitValue.CreatePercentValue(100));

                // Add the headers with style
                tableBillsByBusiness.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Id Factura").AddStyle(headerStyleTable)).AddStyle(lightGrayStyle));
                tableBillsByBusiness.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Fecha").AddStyle(headerStyleTable)).AddStyle(lightGrayStyle));
                tableBillsByBusiness.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Descuento (%)").AddStyle(headerStyleTable)).AddStyle(lightGrayStyle));
                tableBillsByBusiness.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Subtotal ($)").AddStyle(headerStyleTable)).AddStyle(lightGrayStyle));
                tableBillsByBusiness.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Total ($)").AddStyle(headerStyleTable)).AddStyle(lightGrayStyle));
                tableBillsByBusiness.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Cliente").AddStyle(headerStyleTable)).AddStyle(lightGrayStyle));
                tableBillsByBusiness.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Empleado").AddStyle(headerStyleTable)).AddStyle(lightGrayStyle));

                tableBillsByBusiness.AddStyle(tableDataStyle);

                int cont = 1;

                // Add the data
                foreach (BillsByBusinessModel bill in _reportsService.GetBillsByBusiness(reportsView.StartDate, reportsView.EndDate, sale.IdBusiness))
                {
                    //Every other row is gray
                    if (cont % 2 == 0)
                    {
                        tableBillsByBusiness.AddCell(new Cell(1, 1).Add(new Paragraph(bill.IdBill.ToString())).AddStyle(lightGrayStyle));
                        tableBillsByBusiness.AddCell(new Cell(1, 1).Add(new Paragraph(bill.Date.ToString(("dd/MM/yyyy")))).AddStyle(lightGrayStyle));
                        tableBillsByBusiness.AddCell(new Cell(1, 1).Add(new Paragraph(bill.PercentDiscount.ToString())).AddStyle(lightGrayStyle));
                        tableBillsByBusiness.AddCell(new Cell(1, 1).Add(new Paragraph(bill.SubTotal.ToString())).AddStyle(lightGrayStyle));
                        tableBillsByBusiness.AddCell(new Cell(1, 1).Add(new Paragraph(bill.TotalCost.ToString())).AddStyle(lightGrayStyle));
                        tableBillsByBusiness.AddCell(new Cell(1, 1).Add(new Paragraph(bill.ClientName)).AddStyle(lightGrayStyle));
                        tableBillsByBusiness.AddCell(new Cell(1, 1).Add(new Paragraph(bill.EmployeeName)).AddStyle(lightGrayStyle));
                    }
                    else
                    {
                        tableBillsByBusiness.AddCell(bill.IdBill.ToString());
                        tableBillsByBusiness.AddCell(bill.Date.ToString(("dd/MM/yyyy")));
                        tableBillsByBusiness.AddCell(bill.PercentDiscount.ToString());
                        tableBillsByBusiness.AddCell(bill.SubTotal.ToString());
                        tableBillsByBusiness.AddCell(bill.TotalCost.ToString());
                        tableBillsByBusiness.AddCell(bill.ClientName);
                        tableBillsByBusiness.AddCell(bill.EmployeeName);
                    }
                    cont++;
                }
                // Add the table to the document
                doc.Add(tableBillsByBusiness);

                // Add a page break if there are more clients
                if (sale != salesByBusiness.Last())
                {
                    doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                }
            }

            // Close the document
            doc.Close();

            // Return the stream
            return new MemoryStream(stream.ToArray());
        }

        private MemoryStream ExportTotalPurchasesPDF(ReportsViewModel reportsView)
        {
            throw new NotImplementedException();
        }
    }
}

