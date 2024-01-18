//-------------------------------------------------------------------------------------//
//                                  Client                                             //
//-------------------------------------------------------------------------------------//
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
            //redirect to the same page
            window.location.href = '/Facturation/Purchase';
        })
        .catch(error => {
            console.error('Error al obtener los departamentos:', error);
        });
}

//-------------------------------------------------------------------------------------//
//                                  Products                                             //
//-------------------------------------------------------------------------------------//
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
            applyEventListenersToRow();
        })


}


function addRow(tableBodyHtml) {
    var tableContainer = document.getElementById('tableContainer');
    var last = document.getElementById('productsTable');
    if (last != null) {
        tableContainer.removeChild(last)
    }
    tableContainer.innerHTML += tableBodyHtml; // Esto agregará la fila al final de tu tabla
}


function applyEventListenersToRow() {
    var editableCells = document.querySelectorAll('.editableCell');
    var editableInputs = document.querySelectorAll('.editableInput');

    var deliveryModal = new bootstrap.Modal(document.getElementById('modalDelivery'));
    var assignDelivery = document.querySelector('#assignDelivery');
    if (assignDelivery) {
        assignDelivery.addEventListener('click', function () {
            var idAddressClient = document.getElementById('idAddressClient').value;

            if (idAddressClient == '') {
                //swall alert
                Swal.fire({
                    title: 'Error',
                    text: 'Debe seleccionar un cliente para obtener su direccion',
                    icon: 'error'
                })
            } else {

                deliveryModal.show();
            }

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
                div.textContent = '$ ' +input.value ; // Use textContent instead of innerHTML
            } else {               
                div.textContent = input.value; // Use textContent instead of innerHTML

                var idProduct = this.getAttribute('data-idProduct');
                if (idProduct != null && idProduct != ''){
                    //conver the input value to int
                    var inputValue = this.value;
                    if (checkGreterThanZero(inputValue)) {
                        UpdateQuanty(idProduct, inputValue);
                    }
                }
            }
            
            input.style.display = 'none'; // Hide the input
            div.style.display = 'block'; // Show the div

           
        });

        input.addEventListener('keypress', function (event) {
            if (event.key === 'Enter') {
                this.blur();

            }
        });
    });

    var removeItemButton = document.querySelectorAll('.removeItemButton');

    removeItemButton.forEach(function (button) {
        button.addEventListener('click', function () {
            var id = button.getAttribute('data-id');
            removeProductFromCart(id);
        });
    });

}




function checkGreterThanZero(inputValue) {
    if (inputValue === '' || inputValue === null) return false;
    
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

function UpdateQuanty(idProduct, quanty) {
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
//                                  Delevery                                           //
//-------------------------------------------------------------------------------------//
export function GetTypeDeliveries() {
    return fetch('/Facturation/Purchase/GetTypeDeliveries', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        }
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(jsonData => {
            return jsonData;
        });
}

export function GetCompanyTrans() {
    return fetch('/Facturation/Purchase/GetCompanyTrans', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        }
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(jsonData => {
            return jsonData;
        });
}



export { assignClientToBill, AddProductToCart, removeProductFromCart, applyEventListenersToRow };