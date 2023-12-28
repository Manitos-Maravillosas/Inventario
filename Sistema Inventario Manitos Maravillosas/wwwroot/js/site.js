// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




    document.getElementById('searchInput').addEventListener('keyup', function() {
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



