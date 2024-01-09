//Json Structure of a billData

//{
//    "IdBill": 0,
//        "Date": "2024-01-09T07:08:41.182531",
//            "PercentDiscount": 0.0,
//                "SubTotal": 0.0,
//                    "TotalCost": 0.0,
//                        "IdEmployee": "defaultEmployeeId",
//                            "Employee": null,
//                                "IdClient": "defaultClientId",
//                                    "Client": null,
//                                        "IdBusiness": 1,
//                                            "Business": null,
//                                                "CartXProducts": []
//}


document.addEventListener('DOMContentLoaded', function () {


    //-------------------------------------------------------------------------------------//
    //                                  Client Modal                                       //
    //-------------------------------------------------------------------------------------//
    var assignClient = document.querySelector('#assignClient');

    // modals
    var clientModal = new bootstrap.Modal(document.getElementById('modalClient'));
    var newClient = new bootstrap.Modal(document.getElementById('addClient'));
    var deliveryModal = new bootstrap.Modal(document.getElementById('modalDelivery'));
    
    assignClient.addEventListener('click', function () {        
        clientModal.show();
    });

    var openNewClient = document.querySelector('#openNewClient');

    if (openNewClient) {
        openNewClient.addEventListener('click', function () {

            clientModal.hide();
            
            newClient.show();
        });
    }
 
    //-------------------------------------------------------------------------------------//
    //                                  Delevery Modal                                      //
    //-------------------------------------------------------------------------------------//
    var assignDelivery = document.querySelector('#assignDelivery');
    if (assignDelivery) {
        assignDelivery.addEventListener('click', function () {

            deliveryModal.show();
        });
    }


    //-------------------------------------------------------------------------------------//
    //                           AddRow For services (editableRow)                          //
    //-------------------------------------------------------------------------------------//

    var addRowButton = document.getElementById('addRowButton');
    var table = document.getElementById('productsBody');
    //var editableRow = table.getElementsByClassName('editableRow');

    if (addRowButton != 0 && table != null) {
        addRowButton.addEventListener('click', function () {
            var newRow = editableRow[0].cloneNode(true)
            newRow.classList.remove('d-none'); // Remove the editableRow class
            applyEventListenersToRow(newRow); // Apply event listeners to the new row

            addRow(table, newRow, 0); // Add the new row to the table)

            // Clear the text and values in the cloned row
            var inputs = newRow.querySelectorAll('.editableInput');

            inputs.forEach(function (input) {
                input.value = '0'; // Clear value of input
            });
        });
    } else {
        console.log('Some problem in html');
    }


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
                //var newRow = firstRow.cloneNode(true); // Clone the first row
                //newRow.classList.remove('d-none'); // Remove the editableRow class

                addProductToCart(); // Llama a la función addProductToCart si es 'Enter'

            }
        });
    }
    
    function addProductToCart() {
        var productId = document.getElementById('idProductField').value;
        fetch('/Facturation/Purcharse/AddProductToCart', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: `id=${encodeURIComponent(productId)}&quantity=1`
        })
            .then(response => {
                // Asegúrate de que la respuesta esté bien antes de intentar leerla
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.text(); // Usa response.text() ya que esperas HTML como respuesta
             })
            .then(tableBodyHtml => {
                // 'html' es el HTML de tu PartialView
                // Inserta este HTML en la tabla o donde necesites actualizar

                console.log(tableBodyHtml);
                var table = document.getElementById('productsTable');
                var last = document.getElementById('productsBody');
                if (last != null) {
                    table.removeChild(last)
                }
                table.innerHTML += tableBodyHtml; // Esto agregará la fila al final de tu tabla
                noProductsVisibleFalse()
            })
            //.then(response => response.json())
            //.then(data => {
            //    console.log(data)
            //    if (data.success) {
            //        console.log(data.data);
            //    } else {
            //        var validation = document.getElementById('prductValidadtion');
            //        validation.textContent = data.innerExeption;
            //        validation.classList.remove('d-none');
            //    }
            //})

    }

});

function addRow(table, newRow, billData) {

    // Find the Subtotal row
    var subtotalRow = Array.from(table.getElementsByTagName('tr')).find(row => {
        var cells = row.getElementsByTagName('td');
        return cells.length > 0 && cells[0].textContent.trim() === 'Subtotal';
    });

    //apply data to row
    var cells = newRow.getElementsByTagName('td');
    var id = newRow.getElementsByTagName('th');

    id[0].textContent = JsonData.idProduct;
    cells[0].textContent = JsonData.name;
    cells[1].textContent = JsonData.description;
    cells[2].textContent = JsonData.price;
    cells[4].textContent = JsonData.price * 1;


    // Insert the new row before the Subtotal row
    if (subtotalRow) {
        table.insertBefore(newRow, subtotalRow);
    } else {
        // Fallback in case Subtotal row is not found
        table.appendChild(newRow);
    }



    noProductsVisibleFalse();
}




function noProductsVisibleFalse() {
    var noProducts = document.getElementById('noProducts');
    var productsBody = document.getElementById('productsBody');
    productsBody.classList.remove('d-none');
    noProducts.classList.add('d-none');
}

// Function to add a new non-editable row
function addNonEditableRow(JsonData) {

    
    var cells = newRow.getElementsByTagName('td');
    var id = newRow.getElementsByTagName('th');
    id[0].textContent = JsonData.idProduct;
    cells[0].textContent = JsonData.name;
    cells[1].textContent = JsonData.description;
    cells[2].textContent = JsonData.price;
    cells[4].textContent = JsonData.price * 1;

    var cantCell = newRow.getElementsByClassName('editableCell')[0];
    cantCell.textContent = 1;

    applyEventListenersToRow(newRow); // Apply event listeners to the new row)
    // Find the Subtotal row
    var subtotalRow = Array.from(table.getElementsByTagName('tr')).find(row => {
        var cells = row.getElementsByTagName('td');
        return cells.length > 0 && cells[0].textContent.trim() === 'Subtotal';
    });

    // Insert the new row before the Subtotal row
    if (subtotalRow) {
        table.insertBefore(newRow, subtotalRow);
    } else {
        // Fallback in case Subtotal row is not found
        table.appendChild(newRow);
    }

    noProductsVisibleFalse();
}
function applyEventListenersToRow(row) {
    var editableCells = row.querySelectorAll('.editableCell');
    var editableInputs = row.querySelectorAll('.editableInput');

    editableCells.forEach(function (cell) {
        cell.addEventListener('dblclick', function () {
            var parent = cell.parentElement;
            var input = parent.querySelector('.editableInput');
            input.value = cell.textContent.replace(/[^0-9.]/g, ''); // Use textContent instead of innerHTML
            input.style.display = 'block'; // Show the input
            cell.style.display = 'none'; // Hide the div
            input.focus();
        });
    });

    editableInputs.forEach(function (input) {
        input.addEventListener('blur', function () {
            var parent = input.parentElement;
            var div = parent.querySelector('.editableCell');
            if (input.value === '') {
                input.value = 0;
            }
            if (div.hasAttribute('data-price')) {
                div.textContent = input.value + ' $'; // Use textContent instead of innerHTML
            } else {
                div.textContent = input.value; // Use textContent instead of innerHTML
            }
            
            input.style.display = 'none'; // Hide the input
            div.style.display = 'block'; // Show the div
        });
    });
}

