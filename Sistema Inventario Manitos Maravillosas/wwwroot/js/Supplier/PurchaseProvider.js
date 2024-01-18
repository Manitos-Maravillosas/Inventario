﻿document.addEventListener("DOMContentLoaded", function () {
    var savedProductName = localStorage.getItem('selectedProductName');
    if (savedProductName) {
        document.getElementById('productNameInput').value = savedProductName;
        document.getElementById('hiddenProductName').value = savedProductName;
    }
    var assignProductBtn = document.getElementById('assignProduct');
    var modalProduct = new bootstrap.Modal(document.getElementById('modalProduct'));
    assignProductBtn.addEventListener('click', function () {
        modalProduct.show();
    });

    // Filtrar producto
    document.getElementById('identificationProductFilter').addEventListener('input', function () {
        var inputVal = this.value.toLowerCase();
        var listItems = document.querySelectorAll('#listProduct .list-group-item');
        listItems.forEach(function (item) {
            var productId = item.getAttribute('data-id').toLowerCase();
            var productName = item.getAttribute('data-name').toLowerCase();
            if (productId.includes(inputVal) || productName.includes(inputVal)) {
                item.style.display = '';
            } else {
                item.style.display = 'none';
            }
        });
    });

    // Seleccionar producto 
    var listProduct = document.getElementById('listProduct');
    listProduct.addEventListener('click', function (event) {
        var clickedElement = event.target.closest('li');
        if (clickedElement) {
            var productName = clickedElement.getAttribute('data-name');
            // Guardar en Local Storage
            localStorage.setItem('selectedProductName', productName);

            document.getElementById('productNameInput').value = productName;
            document.getElementById('hiddenProductName').value = productName;

            modalProduct.hide();
            
        }
    });
    localStorage.removeItem('selectedProductName');
       
});
