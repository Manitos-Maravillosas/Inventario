document.getElementById('searchInput').addEventListener('keyup', function () {
    var searchTerm = this.value.toLowerCase();
    var tableRows = document.getElementById('clients').getElementsByTagName('tr');

    for (var i = 1; i < tableRows.length; i++) {
        var row = tableRows[i];
        var rowText = row.textContent.toLowerCase();
        row.style.display = rowText.includes(searchTerm) ? '' : 'none';
    }
});