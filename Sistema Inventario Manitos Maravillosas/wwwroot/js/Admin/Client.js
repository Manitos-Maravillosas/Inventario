// --------------------------- Client Table Filter ---------------------------//

var selectedColumn = -1; // -1 representa todas las columnas

// Cambia la columna seleccionada y actualiza dropdown
document.querySelectorAll('.custom-dropdown-item').forEach(function (item) {
    item.addEventListener('click', function (event) {
        event.preventDefault();
        selectedColumn = parseInt(this.getAttribute('data-column'));
        document.getElementById('dropdownMenuButton').textContent = this.textContent;

        sortAndFilterTable(selectedColumn);
    });
});
function sortAndFilterTable(columnIndex) {
    var table = document.getElementById('clients');
    var rows = Array.from(table.getElementsByTagName('tr')).slice(1);

    if (columnIndex >= 0) {
        // Ordena filas con columna seleccionada
        rows.sort(function (a, b) {
            var cellA = a.cells[columnIndex].textContent.trim().toLowerCase();
            var cellB = b.cells[columnIndex].textContent.trim().toLowerCase();
            return cellA.localeCompare(cellB);
        });
    }

    rows.forEach(function (row) {
        table.appendChild(row);
    });

    if (columnIndex !== -1) {
        rows.forEach(function (row) {
            row.style.display = '';
        });
    } else {
        rows.forEach(function (row) {
            row.style.display = 'none';
        });
    }
    filterTable();
}

document.getElementById('searchInput').addEventListener('keyup', function () {
    filterTable();
});

function filterTable() {
    var searchTerm = document.getElementById('searchInput').value.toLowerCase();
    var tableRows = document.getElementById('clients').getElementsByTagName('tr');

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

        row.style.display = displayRow ? '' : 'none';
    }
}

document.getElementById('searchInput').addEventListener('keyup', function () {
    var searchTerm = this.value.toLowerCase();
    var tableRows = document.getElementById('clients').getElementsByTagName('tr');

    for (var i = 1; i < tableRows.length; i++) {
        var row = tableRows[i];
        row.style.display = 'none'; // Oculta inicialmente todas las filas

        // Busca en todas las celdas de la fila
        for (var j = 0; j < row.cells.length; j++) {
            if (row.cells[j].textContent.toLowerCase().indexOf(searchTerm) > -1) {
                row.style.display = ''; // Muestra la fila si el término de búsqueda coincide
                break;
            }
        }
    }
});