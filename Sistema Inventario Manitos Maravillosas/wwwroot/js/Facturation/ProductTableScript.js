import { AddProductToCart, applyEventListenersToRow, removeProductFromCart } from './facturationService.js';
document.addEventListener('DOMContentLoaded', function () {



    

    //-------------------------------------------------------------------------------------//
    //                           AddRow For services (editableRow)                          //
    //-------------------------------------------------------------------------------------//

    //var addRowButton = document.getElementById('addRowButton');
    //var table = document.getElementById('productsBody');
    ////var editableRow = table.getElementsByClassName('editableRow');

    //if (addRowButton != 0 && table != null) {
    //    addRowButton.addEventListener('click', function () {
    //        var newRow = editableRow[0].cloneNode(true)
    //        newRow.classList.remove('d-none'); // Remove the editableRow class
    //        applyEventListenersToRow(newRow); // Apply event listeners to the new row

    //        addRow(table, newRow, 0); // Add the new row to the table)

    //        // Clear the text and values in the cloned row
    //        var inputs = newRow.querySelectorAll('.editableInput');

    //        inputs.forEach(function (input) {
    //            input.value = '0'; // Clear value of input
    //        });
    //    });
    //} else {
    //    console.log('Some problem in html');
    //}


    //-------------------------------------------------------------------------------------//
    //                                  Product into row                                   //
    //-------------------------------------------------------------------------------------//
    var field = document.getElementById('idProductField');
    var table = 2//document.getElementById('productsBody');
    var firstRow = 2//table.querySelector("tr:first-child"); // Get the first row as a template
    

    // Agrega el event listener al campo de entrada del producto

    if (field != 0 && table != null && firstRow != null) {
        field.addEventListener('keypress', function (e) {
            if (e.key === 'Enter') { // Verifica si la tecla presionada es 'Enter'
                AddProductToCart(); // Llama a la función addProductToCart si es 'Enter'

            }
        });
    }


    
    applyEventListenersToRow();

});

function removeItemFromCart(id) {
    removeProductFromCart(id);
}






