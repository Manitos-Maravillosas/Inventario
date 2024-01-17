document.addEventListener('DOMContentLoaded', function () {
    var addonMoney = document.querySelectorAll('.addonMoney');
    var usdRadio = document.getElementById('usdRadio');
    var nicRadio = document.getElementById('nicRadio');

    var exchangeMoney = document.getElementById('exchangeMoney');
    usdRadio.addEventListener('change', function () {
        if (usdRadio.checked) {
            addonMoney.forEach(function (item) {
                item.innerHTML = '$';
            });

            ConvertMoney(12, exchangeMoney.value);
        }
    });

    nicRadio.addEventListener('change', function () {
        if (nicRadio.checked) {
            addonMoney.forEach(function (item) {
                item.innerHTML = 'C$';
            });
        }
        ConvertMoney(658, exchangeMoney.value);
    });


    //option 658 = USD -> C$
    //option 12 = C$ -> USD
    function ConvertMoney(option, val) {
        fetch('/Facturation/Purchase/ConvertMoney', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: `option=${encodeURIComponent(option)}&value=${encodeURIComponent(val)}`
        }).then(response => {
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
});


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
                div.textContent = '$ ' + input.value; // Use textContent instead of innerHTML
            } else {
                div.textContent = input.value; // Use textContent instead of innerHTML

                var idProduct = this.getAttribute('data-idProduct');
                if (idProduct != null && idProduct != '') {
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
