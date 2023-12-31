﻿document.addEventListener('DOMContentLoaded', function () {


    // Modal handling
    var assignClient = document.querySelector('#assignClient');
    assignClient.addEventListener('click', function () {
        var clientModal = new bootstrap.Modal(document.getElementById('modalClient'));
        clientModal.show();
    });

    var assignDelivery = document.querySelector('#assignDelivery');
    assignDelivery.addEventListener('click', function () {
        var deliveryModal = new bootstrap.Modal(document.getElementById('modalDelivery'));
        deliveryModal.show();
    });

    var editableCell = document.querySelectorAll('.editableCell');
    var editableInput = document.querySelectorAll('.editableInput');
    
    editableCell.forEach(function (cell) {
        cell.addEventListener('dblclick', function () {
            var parent = cell.parentElement;
            var input = parent.querySelector('.editableInput');
            input.value = cell.textContent; // Use textContent instead of innerHTML
            input.style.display = 'block'; // Show the input
            cell.style.display = 'none'; // Hide the div
            input.focus();
        });
    });

    editableInput.forEach(function (input) {
        input.addEventListener('blur', function () {
            var parent = input.parentElement;
            var div = parent.querySelector('.editableCell');
            if (input.value === '') {
                input.value = 0;
            }
            div.textContent = input.value; // Use textContent instead of innerHTML
            input.style.display = 'none'; // Hide the input
            div.style.display = 'block'; // Show the div
        });
    });
    document.getElementById('addRowButton').addEventListener('click', function () {
        var table = document.getElementById('productsBody');
        var newRow = table.getElementsByClassName('editableRow')[0].cloneNode(true);
        newRow.classList.remove('d-none'); // Remove the editableRow class
        applyEventListenersToRow(newRow); // Apply event listeners to the new row

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

        // Clear the text and values in the cloned row
        var editableDivs = newRow.querySelectorAll('.editableCell');
        var inputs = newRow.querySelectorAll('.editableInput');

        inputs.forEach(function (input) {
            input.value = '0'; // Clear value of input
        });

        noProductsVisibleFalse();
    });

    

});

function noProductsVisibleFalse() {
    var noProducts = document.getElementById('noProducts');
    var productsBody = document.getElementById('productsBody');
    productsBody.classList.remove('d-none');
    noProducts.classList.add('d-none');
}

// Function to add a new non-editable row
function addNonEditableRow(JsonData) {
    var table = document.getElementById('productsBody');
    var firstRow = table.querySelector("tr:first-child"); // Get the first row as a template
    var newRow = firstRow.cloneNode(true); // Clone the first row
    newRow.classList.remove('d-none'); // Remove the editableRow class
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

function addProductToCart() {
    var productId = document.getElementById('idProduct').value;
    console.log(productId);
    fetch('/Facturation/Purcharse/AddProductToCart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: `id=${encodeURIComponent(productId)}&quantity=1`
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {                
                addNonEditableRow(data.data)
            } else {
                var validation = document.getElementById('prductValidadtion');
                validation.textContent = data.innerExeption;
                validation.classList.remove('d-none');
            }
        })
}




