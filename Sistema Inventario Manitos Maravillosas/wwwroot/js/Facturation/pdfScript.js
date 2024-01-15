// The URL of the action that generates the PDF
document.addEventListener('DOMContentLoaded', function () {

   

    var pdfDoc = null;

    // Load the PDF file.
    pdfjsLib.getDocument(pdfUrl).promise.then(function (pdf) {
        pdfDoc = pdf;
        // Get the first page.
        pdf.getPage(1).then(function (page) {
            var canvas = document.getElementById('pdf-canvas');
            var context = canvas.getContext('2d');
            var viewport = page.getViewport({ scale: 1.5 });

            canvas.height = viewport.height;
            canvas.width = viewport.width;

            // Render the page.
            var renderContext = {
                canvasContext: context,
                viewport: viewport
            };
            page.render(renderContext);
        });
    });


    // Download functionality
    document.getElementById('download-button').addEventListener('click', function () {
        if (pdfDoc) {
            var link = document.createElement('a');
            link.href = pdfUrl;
            link.download = 'GeneratedInvoice.pdf';
            link.click();
        }
    });
});
