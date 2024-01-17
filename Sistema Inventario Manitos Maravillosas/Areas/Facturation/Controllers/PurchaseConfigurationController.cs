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
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Helper;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Inventario_Manitos_Maravillosas.Areas.AdminPayment.Data.Services;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Controllers
{

    [Area("Facturation")]
    [Authorize]
    public class PurchaseConfigurationController : Controller
    {

        private readonly IProductService _productService;
        private readonly IClientService _clientService;
        private readonly BillHandler _billHandler;

        private readonly ICoinService _CoinService;
        private readonly ITypePaymentService _typePaymentService;
        private readonly IBankAccountService _bankAccountService;
        private Bill bill;
        public PurchaseConfigurationController(IProductService productService, IClientService clientService, ITypePaymentService typePaymentService,
            IBankAccountService bankAccountService,  BillHandler billHandler, ICoinService coinService)
        {

            _productService = productService;
            _clientService = clientService;
            _billHandler = billHandler;
            _typePaymentService = typePaymentService;
            _bankAccountService = bankAccountService;
            _CoinService = coinService;
        }

        public IActionResult Index()
        {
            bill = _billHandler.GetBill();
            // The JSON string
            string json = "{\"idBill\":0,\"date\":\"2024-01-16T21:42:57.6021551-06:00\",\"percentDiscount\":0,\"amountDiscount\":0,\"subTotal\":1020,\"totalCost\":1023,\"idEmployee\":\"defaultEmployeeId\",\"employee\":null,\"idClient\":\"109710812\",\"client\":{\"id\":\"109710812\",\"name\":\"Ingrid\",\"lastName1\":\"Mena\",\"lastName2\":\"Barboza\",\"phoneNumber\":\"84252989\",\"idAddress\":14,\"departmentName\":\"Chinandega\",\"cityName\":\"El Realejo\",\"signs\":\"que le importa\"},\"idBusiness\":1,\"business\":null,\"cartXProducts\":[{\"idCartXProduct\":0,\"quantity\":4,\"cost\":50,\"price\":55,\"subTotal\":220,\"idProduct\":\"321\",\"product\":{\"idProduct\":\"321\",\"productName\":\"Headphones\",\"stock\":50,\"cost\":50,\"price\":55,\"description\":\"Wireless headphones with noise cancellation.\",\"status\":true,\"idBusiness\":1,\"business\":null,\"idProductCategory\":1,\"category\":\"Electronics\"},\"idBill\":0,\"bill\":null},{\"idCartXProduct\":0,\"quantity\":8,\"cost\":50,\"price\":60,\"subTotal\":480,\"idProduct\":\"987987\",\"product\":{\"idProduct\":\"987987\",\"productName\":\"Naranja\",\"stock\":80,\"cost\":50,\"price\":60,\"description\":\"Producto citrico de calidad\",\"status\":true,\"idBusiness\":1,\"business\":null,\"idProductCategory\":7,\"category\":\"Frutas y verduras\"},\"idBill\":0,\"bill\":null},{\"idCartXProduct\":0,\"quantity\":8,\"cost\":35,\"price\":40,\"subTotal\":320,\"idProduct\":\"456\",\"product\":{\"idProduct\":\"456\",\"productName\":\"Garden Hose\",\"stock\":40,\"cost\":35,\"price\":40,\"description\":\"Flexible 50ft garden hose with adjustable nozzle.\",\"status\":true,\"idBusiness\":2,\"business\":null,\"idProductCategory\":2,\"category\":\"Gardening Tools\"},\"idBill\":0,\"bill\":null}],\"products\":[],\"optionMoney\":1,\"listClients\":null,\"deliveryFlag\":true,\"delivery\":{\"id\":0,\"total\":3,\"internalCost\":3,\"notes\":null,\"dateAprox\":\"2024-01-20T00:00:00\",\"signs\":null,\"nameTypeDelivery\":null,\"idAddress\":0,\"idTypeDelivery\":1,\"idBill\":0,\"address\":null,\"typeDelivery\":null,\"bill\":null,\"deliveryxCompanyTrans\":{\"aditionalCompanyCost\":0,\"idCompanyTrans\":0,\"inChargePaymentDelivery\":\"2\"}}}";

            var coinDescriptions = _CoinService.GetCoinDescriptions();
            ViewBag.CoinDescriptions = new SelectList(coinDescriptions);
            // Deserialize the JSON string to a Bill object
            bill = JsonConvert.DeserializeObject<Bill>(json);
            _billHandler.SaveBill(bill);
            if (bill.CartXProducts.Count <= 0)
            {
                return RedirectToAction("Index", "Purchase");
            }
            else
            {
                return View();                
            }

        }


        public IActionResult GeneratePdf()
        {
            bill = _billHandler.GetBill();

            // Calculate the total height required for the content
            float totalContentHeight = CalculateContentHeight(bill.CartXProducts);

            


            MemoryStream stream = new MemoryStream();
            PdfWriter writer = new PdfWriter(stream);

            // Define custom page size - 80mm width
            float mmToPoints = 2.83465f; // 1 mm in points
            //float pageWidth = 80 * mmToPoints; // 80 mm in points
            //float pageHeight = 500 * mmToPoints; // An arbitrary height, adjust as necessary
            //var pageSize = new PageSize(pageWidth, pageHeight);

            // Define custom page size
            float pageWidth = 80 * mmToPoints; // Width for the thermal printer paper
            totalContentHeight  = totalContentHeight * mmToPoints; // Convert the height to points
            var pageSize = new PageSize(pageWidth, totalContentHeight); // Use the calculated height

            // Set the custom page size for the PDF document
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, pageSize);

            // Set margins if necessary, can be adjusted as needed
            document.SetMargins(10, 0, 0, 0);





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
            document.Add(new Paragraph("Dir: Bello Horizonte del tope sur de la rotonda,1c al lago, 1c arriba 1c al lago casa color rojo")
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

            foreach (var item in bill.CartXProducts)
            {
                // Add rows of items with no borders
                tableProducts.AddCell(new Cell().Add(new Paragraph(item.Product.ProductName)).AddStyle(noBorderStyle));
                tableProducts.AddCell(new Cell().Add(new Paragraph(item.Quantity.ToString())).AddStyle(noBorderStyle));
                tableProducts.AddCell(new Cell().Add(new Paragraph("$ "+item.Price.ToString())).AddStyle(noBorderStyle));
                tableProducts.AddCell(new Cell().Add(new Paragraph("$ "+item.SubTotal.ToString())).AddStyle(noBorderStyle));
            }

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
        float CalculateContentHeight(IEnumerable<CartXProduct> collection)
        {
            float headaer = 114f;
            float height = headaer+58;
            foreach (var item in collection)
            {
                // Estimate the height each item will add to the document
                // This can be a fixed value or based on the content of the item
                height += 5;
            }
            return height;
        }


    }
    public class PrintPdfViewModel
    {
        public string PdfUrl { get; set; }
    }

}
