using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

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
            // Create a new memory stream.
            using (var stream = new MemoryStream())
            {
                // Create a writer for the stream.
                using (var writer = new PdfWriter(stream))
                {
                    // Create a new PDF document.
                    using (var pdf = new PdfDocument(writer))
                    {
                        // Create a document to add content to the PDF.
                        var document = new Document(pdf);

                        // Add a paragraph to the document.
                        document.Add(new Paragraph("Hello, world!"));

                        // Close the document to finalize the PDF.
                        document.Close();
                    }

                    // Convert the memory stream to a byte array.
                    var bytes = stream.ToArray();

                    // Return the PDF as a FileResult.
                    return File(bytes, "application/pdf", "Document.pdf");
                }
            }
        }

        

    }


}
