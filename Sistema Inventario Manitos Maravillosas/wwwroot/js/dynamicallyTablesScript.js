// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {

    var table = document.getElementsByClassName('dynamicallyTable');
    var currentPage = 1; // Keeps track of the current page
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
                    currentPage = totalPages; // Wrap to last page if on the first page
                }
                updateTableRows(currentPage, showCantRow.value, tableBody);
            });

            nextButton.addEventListener('click', function () {
                var totalRows = tableBody.querySelectorAll('tr').length;
                var totalPages = Math.ceil(totalRows / showCantRow.value);

                if (currentPage < totalPages) {
                    currentPage++;
                } else {
                    currentPage = 1; // Wrap to first page if on the last page
                }
                updateTableRows(currentPage, showCantRow.value, tableBody);
            });
        }
    }
    applyShowRows(10, tableBody)
    updateCarousel(tableBody, 10);


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
    updateCarousel(tableBody, cant);
}
function updateCarousel(tableBody, cant) {
    var allRows = tableBody.querySelectorAll('tr').length;
    var totalPages = Math.ceil(allRows / cant);

    var carouselInner = document.querySelector('.carousel-inner');
    carouselInner.innerHTML = ''; // Clear existing carousel items

    for (var i = 1; i <= totalPages; i++) {
        var carouselItem = document.createElement('div');
        carouselItem.className = 'carousel-item' + (i === 1 ? ' active' : '');
        carouselItem.innerHTML = `<div class="d-flex justify-content-center align-items-center"><span>${i}</span></div>`;

        carouselInner.appendChild(carouselItem);
    }

    // Optionally, handle the display of carousel controls based on the number of pages
    // For example, hide them if there's only one page
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
            row.style.display = ''; // Show row
        } else {
            row.style.display = 'none'; // Hide row
        }
    });
}

