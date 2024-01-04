// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    
    var table = document.getElementsByClassName('dynamicallyTable');

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
    }

    
});

function applyShowRows(cant, tableBody) {
    var allRows = tableBody.querySelectorAll('tr');
    var totalRows = allRows.length;

    // Hide all rows
    allRows.forEach(function (row) {
        row.style.display = 'none';
    });

    // Show only the number of rows selected by the user
    for (var i = 0; i < cant && i < totalRows; i++) {
        allRows[i].style.display = '';
    }
}
