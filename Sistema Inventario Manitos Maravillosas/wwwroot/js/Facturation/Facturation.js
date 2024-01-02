document.addEventListener('DOMContentLoaded', function () {


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
        var table = document.getElementById('productsTable').getElementsByTagName('tbody')[0];
        var newRow = table.getElementsByClassName('editableRow')[0].cloneNode(true);
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

        editableDivs.forEach(function (div) {
            div.textContent = ''; // Clear text content of div
        });

        inputs.forEach(function (input) {
            input.value = ''; // Clear value of input
        });
    });

    

});

function applyEventListenersToRow(row) {
    var editableCells = row.querySelectorAll('.editableCell');
    var editableInputs = row.querySelectorAll('.editableInput');

    editableCells.forEach(function (cell) {
        cell.addEventListener('dblclick', function () {
            var parent = cell.parentElement;
            var input = parent.querySelector('.editableInput');
            input.value = cell.textContent; // Use textContent instead of innerHTML
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
            div.textContent = input.value; // Use textContent instead of innerHTML
            input.style.display = 'none'; // Hide the input
            div.style.display = 'block'; // Show the div
        });
    });
}



function addProductToCart() {
    var input = document.getElementById("inputField").value;
    fetch('/YourController/GetData?input=' + input)
        .then(response => response.json())
        .then(data => {
            var table = document.getElementById("dataTable");
            table.innerHTML = ''; // Clear existing rows
            data.forEach(item => {
                // Assuming 'item' has properties 'id' and 'name'
                table.innerHTML += '<tr><td>' + item.id + '</td><td>' + item.name + '</td></tr>';
            });
        });
}
