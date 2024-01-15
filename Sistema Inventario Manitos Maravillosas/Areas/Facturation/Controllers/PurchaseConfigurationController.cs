using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Layout.Borders;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Controllers
{
   
    [Area("Facturation")]
    [Authorize]
    public class PurchaseConfigurationController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult GeneratePdf()
        {
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = new PdfWriter(stream);

            // Define custom page size - 80mm width
            float mmToPoints = 2.83465f; // 1 mm in points
            float pageWidth = 80 * mmToPoints; // 80 mm in points
            float pageHeight = 500 * mmToPoints; // An arbitrary height, adjust as necessary
            var pageSize = new PageSize(pageWidth, pageHeight);

            // Set the custom page size for the PDF document
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, pageSize);

            // Set margins if necessary, can be adjusted as needed
            document.SetMargins(10, 10, 10, 10);




            // Creating font instances
            PdfFont helvetica = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont helveticaBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            // Style for bold text
            // Define the regular style with 12pt font size
            Style regularStyle = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(10);

            // Define the bold style with 12pt font size
            Style boldStyle = new Style()
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetFontSize(10);

            //--------------------------------------------------------------------- Header
            document.Add(new Paragraph("MANITOS MARAVILLOSAS")
                            .AddStyle(boldStyle)
                            .SetTextAlignment(TextAlignment.CENTER));
            // RUC and other details
            document.Add(new Paragraph("RUC: 09052013581")
                            .AddStyle(boldStyle));
            document.Add(new Paragraph("@manitos.maravillosas.ni/@donmaenic")
                            .AddStyle(boldStyle));
            document.Add(new Paragraph("Dir: Bello Horizonte del tope sur de la rotonda,\r\n1c al lago, 1c arriba 1c al lago casa color rojo")
                            .AddStyle(boldStyle));

            // Invoice details
            document.Add(new Paragraph("FACTURA N°: FACT - 1999")
                            .AddStyle(boldStyle));
            document.Add(new Paragraph("Cliente: Daniela Baltodano"));
            document.Add(new Paragraph("Fecha: 31 dic 2023"));
            document.Add(new Paragraph("Tipo de Factura: En Tienda"));
            document.Add(new Paragraph("Atendido: Ericka Arévalo"));


            ////--------------------------------------------------------------------- tableHeader
            Table tableHeader = new Table(UnitValue.CreatePercentArray(new float[] { 3, 1, 1, 1 })).UseAllAvailableWidth();

            // Define a style for cells with no border
            Style noBorderStyle = new Style().SetBorder(Border.NO_BORDER);

            // Add headers with no borders
            tableHeader.AddCell(new Cell().Add(new Paragraph("DESCRIPCION").AddStyle(boldStyle)).AddStyle(noBorderStyle));
            tableHeader.AddCell(new Cell().Add(new Paragraph("CANT").AddStyle(boldStyle)).AddStyle(noBorderStyle));
            tableHeader.AddCell(new Cell().Add(new Paragraph("PRECIO").AddStyle(boldStyle)).AddStyle(noBorderStyle));
            tableHeader.AddCell(new Cell().Add(new Paragraph("TOTAL").AddStyle(boldStyle)).AddStyle(noBorderStyle));


            //--------------------------------------------------------------------- tableProducts
            Table tableProducts = new Table(UnitValue.CreatePercentArray(new float[] { 3, 1, 1, 1 })).UseAllAvailableWidth();

            // Add rows of items with no borders
            tableProducts.AddCell(new Cell().Add(new Paragraph("Guillotina liza azul marca")).AddStyle(noBorderStyle));
            tableProducts.AddCell(new Cell().Add(new Paragraph("1")).AddStyle(noBorderStyle));
            tableProducts.AddCell(new Cell().Add(new Paragraph("300")).AddStyle(noBorderStyle));
            tableProducts.AddCell(new Cell().Add(new Paragraph("$300")).AddStyle(noBorderStyle));

            // Add rows of items with no borders
            tableProducts.AddCell(new Cell().Add(new Paragraph("Guillotina liza azul marca")).AddStyle(noBorderStyle));
            tableProducts.AddCell(new Cell().Add(new Paragraph("1")).AddStyle(noBorderStyle));
            tableProducts.AddCell(new Cell().Add(new Paragraph("300")).AddStyle(noBorderStyle));
            tableProducts.AddCell(new Cell().Add(new Paragraph("$300")).AddStyle(noBorderStyle));

            //--------------------------------------------------------------------- tablePrice
            Table tablePrice = new Table(UnitValue.CreatePercentArray(new float[] { 3, 1, 1, 1 })).UseAllAvailableWidth();

            // IVA ROW
            tablePrice.AddCell(new Cell().Add(new Paragraph("IVA - Incluido").AddStyle(boldStyle)).AddStyle(noBorderStyle));
            tablePrice.AddCell(new Cell().Add(new Paragraph("")).AddStyle(noBorderStyle));
            tablePrice.AddCell(new Cell().Add(new Paragraph("15 %").AddStyle(boldStyle)).AddStyle(noBorderStyle));
            tablePrice.AddCell(new Cell().Add(new Paragraph("$63").AddStyle(boldStyle)).AddStyle(noBorderStyle));

            // Discount ROW
            tablePrice.AddCell(new Cell().Add(new Paragraph("Desceunto").AddStyle(boldStyle)).AddStyle(noBorderStyle));
            tablePrice.AddCell(new Cell().Add(new Paragraph("")).AddStyle(noBorderStyle));
            tablePrice.AddCell(new Cell().Add(new Paragraph("0 %").AddStyle(boldStyle)).AddStyle(noBorderStyle));
            tablePrice.AddCell(new Cell().Add(new Paragraph("$63").AddStyle(boldStyle)).AddStyle(noBorderStyle));

            // Delevery ROW
            tablePrice.AddCell(new Cell().Add(new Paragraph("Envío").AddStyle(boldStyle)).AddStyle(noBorderStyle));
            tablePrice.AddCell(new Cell().Add(new Paragraph("")).AddStyle(noBorderStyle));
            tablePrice.AddCell(new Cell().Add(new Paragraph("Sí").AddStyle(boldStyle)).AddStyle(noBorderStyle));
            tablePrice.AddCell(new Cell().Add(new Paragraph("$435").AddStyle(boldStyle)).AddStyle(noBorderStyle));

            // Total ROW
            tablePrice.AddCell(new Cell().Add(new Paragraph("").AddStyle(boldStyle)).AddStyle(noBorderStyle));
            tablePrice.AddCell(new Cell().Add(new Paragraph("").AddStyle(boldStyle)).AddStyle(noBorderStyle));
            tablePrice.AddCell(new Cell().Add(new Paragraph("TOTAL").AddStyle(boldStyle)).AddStyle(noBorderStyle));
            tablePrice.AddCell(new Cell().Add(new Paragraph("$435").AddStyle(boldStyle)).AddStyle(noBorderStyle));



            // Add table to document
            document.Add(new Paragraph("---------------------------------------------------").AddStyle(boldStyle));
            document.Add(tableHeader);
            document.Add(new Paragraph("---------------------------------------------------").AddStyle(boldStyle));
            document.Add(tableProducts);
            document.Add(new Paragraph("---------------------------------------------------").AddStyle(boldStyle));
            document.Add(tablePrice);

            // Close the document
            document.Close();

            // Return the generated PDF
            var content = stream.ToArray();
            return File(content, "application/pdf", "GeneratedInvoice.pdf");
        }

    }


}
