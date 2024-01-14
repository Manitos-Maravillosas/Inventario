﻿
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

    //filter identification
    document.getElementById('identificationClientFilter').addEventListener('input', function () {
        var inputVal = this.value;
        var listItems = document.querySelectorAll('#listClient .list-group-item');
        if (listItems.length > 0) {
            listItems.forEach(function (item) {
                var clientId = item.getAttribute('data-id');
                if (clientId.includes(inputVal)) {
                    item.style.display = ''; // Show the item if it matches
                } else {
                    item.style.display = 'none'; // Hide the item if it doesn't match
                }
            });
        }
        
    });

    //-----------------------------------------------------select client
    var listClient = document.getElementById('listClient');
    if (listClient) {

        var elements = listClient.getElementsByTagName('li');
        for (var i = 0; i < elements.length; i++) {
            elements[i].addEventListener('click', function () {
                //for (let j = 0; j < elements.length; j++) {
                //    elements[j].classList.remove('active');
                //}

                //// Add 'active' class to the clicked element
                //this.classList.add('active');

                var id = this.getAttribute('data-id');
                var name = this.getAttribute('data-name');
                var idAddress = this.getAttribute('data-idaddress');

                document.getElementById('clientDataInput').value = id + ' - '+name;
                document.getElementById('idAddressClient').value = idAddress;

                assignClientToBill(id);

                clientModal.hide();
            });
        }       
    }
    //-----------------------------------------------------assing client to a bill
    function assignClientToBill(idClient) {
        fetch('/Facturation/Purchase/AssingClientToBill', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: `id=${encodeURIComponent(idClient)}`
        })
            .then(response => {
                // Asegúrate de que la respuesta esté bien antes de intentar leerla
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.text(); // Usa response.text() ya que esperas HTML como respuesta
            })
            .catch(error => {
                console.error('Error al obtener los departamentos:', error);
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
                AddProductToCart(); // Llama a la función addProductToCart si es 'Enter'

            }
        });
    }
    
    function AddProductToCart() {
        var productId = document.getElementById('idProductField').value;
        fetch('/Facturation/Purchase/AddProductToCart', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: `id=${encodeURIComponent(productId)}&quantity=1`
        })
            .then(response => {
                // Check the response header to determine the content type
                const contentType = response.headers.get("content-type");
                if (contentType && contentType.indexOf("application/json") !== -1) {
                    return response.json().then(data => {
                        // Process JSON data
                        if (!data.success) {
                            // Handle failure. Display the error message from the server.
                            Swal.fire({
                                title: 'Error',
                                text: data.message,
                                icon: 'error'
                            });
                        }
                    });
                } else if (contentType && contentType.indexOf("text/html") !== -1) {
                    return response.text().then(html => {
                        // Process HTML data
                        addRow(html);
                        noProductsVisibleFalse();
                        applyEventListenersToRow();
                    });
                } else {
                    throw new TypeError('Received response is neither JSON nor HTML');
                }
            })
            .catch(error => {
                // Handle network errors or other exceptions
                console.error('Fetch Error:', error);
            });
    }
    
    applyEventListenersToRow();

});

function removeProductFromCart(id) {
    fetch('/Facturation/Purchase/removeProductFromCart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: `id=${encodeURIComponent(id)}`
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

            addRow(tableBodyHtml);
            noProductsVisibleFalse()
            applyEventListenersToRow();
        })


}


function addRow(tableBodyHtml) {
    var table = document.getElementById('productsTable');
    var last = document.getElementById('productsBody');
    if (last != null) {
        table.removeChild(last)
    }
    table.innerHTML += tableBodyHtml; // Esto agregará la fila al final de tu tabla
}

function noProductsVisibleFalse() {
    var noProducts = document.getElementById('noProducts');
    var productsBody = document.getElementById('productsBody');
    productsBody.classList.remove('d-none');
    noProducts.classList.add('d-none');
}


function applyEventListenersToRow() {
    var editableCells = document.querySelectorAll('.editableCell');
    var editableInputs = document.querySelectorAll('.editableInput');

    var deliveryModal = new bootstrap.Modal(document.getElementById('modalDelivery'));
    var assignDelivery = document.querySelector('#assignDelivery');
    if (assignDelivery) {
        assignDelivery.addEventListener('click', function () {

            deliveryModal.show();
        });
    }

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

            var idProduct = this.getAttribute('data-idProduct');
            //conver the input value to int
            var inputValue = this.value;
            if (checkGreterThanZero(inputValue)) {
                UpdateQuanty(idProduct, inputValue, input);
            }
        });

        input.addEventListener('keypress', function (event){
            if (event.key === 'Enter') {
                this.blur();
                
            }
        });
    });

}

function checkGreterThanZero(inputValue) {
    if (!isNaN(inputValue)) {
        inputValue = parseInt(inputValue);
        if (inputValue <= 0) {
            Swal.fire({
                title: 'Error',
                text: 'La cantidad ingresada debe ser mayor a 0',
                icon: 'error'
            })
            return false;
        }
        else {
            return true;
        }

    } else {
        Swal.fire({
            title: 'Error',
            text: 'Debe ser un número',
            icon: 'error'
        })
        return false;
    }
}

function UpdateQuanty(idProduct, quanty, input) {
    fetch('/Facturation/Purchase/UpdateQuanty', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: `id=${encodeURIComponent(idProduct)}&quantity=${encodeURIComponent(quanty)}`
    })
        .then(response => {
            // Check the response header to determine the content type
            const contentType = response.headers.get("content-type");
            if (contentType && contentType.indexOf("application/json") !== -1) {
                return response.json().then(data => {
                    // Process JSON data
                    if (!data.success) {
                        // Handle failure. Display the error message from the server.
                        Swal.fire({
                            title: 'Error',
                            text: data.message,
                            icon: 'error'
                        })
                    }
                });
            } else if (contentType && contentType.indexOf("text/html") !== -1) {
                return response.text().then(html => {
                    // Process HTML data
                    addRow(html);
                    noProductsVisibleFalse();
                    applyEventListenersToRow();
                });
            } else {
                throw new TypeError('Received response is neither JSON nor HTML');
            }
        })
        .catch(error => {
            // Handle network errors or other exceptions
            console.error('Fetch Error:', error);
        });
}



//-------------------------------------------------------------------------------------//
//                                  Product into row                                   //
//-------------------------------------------------------------------------------------//