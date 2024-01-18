// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var selectedColumn = -1; 
var originalRows = [];
document.addEventListener('DOMContentLoaded', function () {

    var table = document.getElementsByClassName('dynamicallyTable');
    var currentPage = 1; 
    if (table.length > 0) {
        var dynamicallyTable = table[0];
        var exitsShow = dynamicallyTable.getElementsByClassName('showCantRow');
        var tableBody = dynamicallyTable.querySelector('tbody');
        if (exitsShow.length > 0) {
            var showCantRow = exitsShow[0];
            showCantRow.addEventListener('change', function () {
                var value = showCantRow.value;
                applyShowRows(value, tableBody);
            });
        }

        var prevButton = document.querySelector('.carousel-control-prev');
        var nextButton = document.querySelector('.carousel-control-next');
        if (prevButton != null && nextButton != null) {
            prevButton.addEventListener('click', function () {
                var totalRows = tableBody.querySelectorAll('tr').length;
                var totalPages = Math.ceil(totalRows / showCantRow.value);

                if (currentPage > 1) {
                    currentPage--;
                } else {
                    currentPage = totalPages; 
                }
                updateTableRows(currentPage, showCantRow.value, tableBody);
            });

            nextButton.addEventListener('click', function () {
                var totalRows = tableBody.querySelectorAll('tr').length;
                var totalPages = Math.ceil(totalRows / showCantRow.value);

                if (currentPage < totalPages) {
                    currentPage++;
                } else {
                    currentPage = 1; 
                }
                updateTableRows(currentPage, showCantRow.value, tableBody);
            });
        }
    }
    applyShowRows(10, tableBody)
    updateCarousel(tableBody, 10);

    var tableFilter = document.getElementById('table');
    originalRows = Array.from(tableFilter.getElementsByTagName('tr')).slice(1);
        
    document.getElementById('searchInput').addEventListener('keyup', filterTable);

    document.querySelectorAll('.sort-icon').forEach(function (icon, index) {
        icon.addEventListener('click', function () {
            const tableId = 'table'; 
            const currentIsAscending = icon.classList.contains("th-sort-asc");
            sortTableByColumn(tableId, index, !currentIsAscending);
        });
    });    
});

function applyShowRows(cant, tableBody) {
    var allRows = tableBody.querySelectorAll('tr');
    var totalRows = allRows.length;

    allRows.forEach(function (row) {
        row.style.display = 'none';
    });

    for (var i = 0; i < cant && i < totalRows; i++) {
        allRows[i].style.display = '';
    }
    updateCarousel(tableBody, cant);
}
function updateCarousel(tableBody, cant) {
    var allRows = tableBody.querySelectorAll('tr').length;
    var totalPages = Math.ceil(allRows / cant);

    var carouselInner = document.querySelector('.carousel-inner');
    carouselInner.innerHTML = ''; 

    for (var i = 1; i <= totalPages; i++) {
        var carouselItem = document.createElement('div');
        carouselItem.className = 'carousel-item' + (i === 1 ? ' active' : '');
        carouselItem.innerHTML = `<div class="d-flex justify-content-center align-items-center"><span>${i}</span></div>`;

        carouselInner.appendChild(carouselItem);
    }

    var prevButton = document.querySelector('.carousel-control-prev');
    var nextButton = document.querySelector('.carousel-control-next');
    if (totalPages <= 1) {
        prevButton.style.display = 'none';
        nextButton.style.display = 'none';
    } else {
        prevButton.style.display = '';
        nextButton.style.display = '';
    }
}

function updateTableRows(page, cant, tableBody) {
    var allRows = tableBody.querySelectorAll('tr');
    var start = (page - 1) * parseInt(cant);
    var end = start + parseInt(cant);

    allRows.forEach(function (row, index) {
        if (index >= start && index < end) {
            row.style.display = ''; 
        } else {
            row.style.display = 'none'; 
        }
    });
}

function filterTable() {
    var searchTerm = document.getElementById('searchInput').value.toLowerCase();
    var tableRows = document.getElementById('table').getElementsByTagName('tr');

    for (var i = 1; i < tableRows.length; i++) {
        var row = tableRows[i];
        var displayRow = false;

        for (var j = 0; j < row.cells.length; j++) {
            
            if (row.cells[j].textContent.toLowerCase().includes(searchTerm)) {
                displayRow = true;
                break; 
            }
        }

        row.style.display = displayRow ? "" : "none"; 
    }
}

function sortTableByColumn(tableId, column, asc = true) {
    const dirModifier = asc ? 1 : -1;
    const table = document.getElementById(tableId);
    let tbody = table.querySelector("tbody");

    const rows = Array.from(tbody.querySelectorAll("tr"));

    // Se modifica la función de comparación para manejar números correctamente
    const sortedRows = rows.sort((a, b) => {
        const aColText = a.querySelector(`td:nth-child(${column + 1})`).textContent.trim();
        const bColText = b.querySelector(`td:nth-child(${column + 1})`).textContent.trim();

        // Intenta convertir a números flotantes para comparación numérica
        const aColNumber = parseFloat(aColText);
        const bColNumber = parseFloat(bColText);

        // Verifica si ambos valores son números y si el texto original es un número
        if (!isNaN(aColNumber) && !isNaN(bColNumber) && aColText == aColNumber.toString() && bColText == bColNumber.toString()) {
            return (aColNumber - bColNumber) * dirModifier;
        }

        // De lo contrario, realiza una comparación de cadenas
        return aColText.toLowerCase().localeCompare(bColText.toLowerCase(), undefined, { numeric: true, sensitivity: 'base' }) * dirModifier;
    });

    while (tbody.firstChild) {
        tbody.removeChild(tbody.firstChild);
    }

    tbody.append(...sortedRows);

    // Actualiza las clases para los iconos de ordenamiento
    table.querySelectorAll("th").forEach(th => th.classList.remove("th-sort-asc", "th-sort-desc"));
    table.querySelector(`th:nth-child(${column + 1})`).classList.toggle("th-sort-asc", asc);
    table.querySelector(`th:nth-child(${column + 1})`).classList.toggle("th-sort-desc", !asc);
}


