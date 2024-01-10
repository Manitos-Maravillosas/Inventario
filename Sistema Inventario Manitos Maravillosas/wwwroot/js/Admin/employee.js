// --------------------------- Employee Table Filter ---------------------------//

var selectedColumn = -1; // -1 representa todas las columnas
var originalRows = []; // Almacena una copia de las filas originales

document.addEventListener('DOMContentLoaded', function () {
    // Copia las filas originales al cargar la página
    var table = document.getElementById('employees');
    originalRows = Array.from(table.getElementsByTagName('tr')).slice(1);

    document.querySelectorAll('.custom-dropdown-item').forEach(function (item) {
        item.addEventListener('click', function (event) {
            event.preventDefault();
            selectedColumn = parseInt(this.getAttribute('data-column'));
            document.getElementById('dropdownMenuButton').textContent = this.textContent;

            sortAndFilterTable(selectedColumn);
        });
    });

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.custom-dropdown-item').forEach(function (item) {
            item.addEventListener('click', function (event) {
                event.preventDefault();
                selectedColumn = parseInt(this.getAttribute('data-column'));
                document.getElementById('dropdownMenuButton').textContent = this.textContent;

                sortAndFilterTable(selectedColumn);
            });
        });
    });

    document.getElementById('searchInput').addEventListener('keyup', filterTable);
});


function sortAndFilterTable(columnIndex) {
    var table = document.getElementById('employees');
    var rows = originalRows.slice(); 

    if (columnIndex >= 0) {
        rows.sort(function (a, b) {
            var cellA = a.cells[columnIndex].textContent.trim().toLowerCase();
            var cellB = b.cells[columnIndex].textContent.trim().toLowerCase();
            return cellA.localeCompare(cellB);
        });
    }

    var tableBody = table.querySelector('tbody');
    tableBody.innerHTML = ''; 

    rows.forEach(function (row) {
        tableBody.appendChild(row);
    });

    filterTable();
}

function filterTable() {
    var searchTerm = document.getElementById('searchInput').value.toLowerCase();
    var tableRows = document.getElementById('employees').getElementsByTagName('tr');

    for (var i = 1; i < tableRows.length; i++) {
        var row = tableRows[i];
        var displayRow = false;

        for (var j = 0; j < row.cells.length; j++) {
            if (selectedColumn === -1 || selectedColumn === j) {
                if (row.cells[j].textContent.toLowerCase().includes(searchTerm)) {
                    displayRow = true;
                    break;
                }
            }
        }

        row.classList.toggle('d-none', !displayRow);
    }

}


