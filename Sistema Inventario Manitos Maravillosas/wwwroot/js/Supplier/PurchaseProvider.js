document.addEventListener("DOMContentLoaded", function () {

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

    // Seleccionar producto utilizando delegación de eventos
    var listProduct = document.getElementById('listProduct');
    listProduct.addEventListener('click', function (event) {
        var clickedElement = event.target.closest('li');
        if (clickedElement) {
            var productName = clickedElement.getAttribute('data-name');

            // Actualiza el input visible y el campo oculto con el nombre del producto seleccionado
            document.getElementById('productNameInput').value = productName;
            document.getElementById('hiddenProductName').value = productName;

            modalProduct.hide();
        }
    });

    // No es necesario manejar el evento 'submit' si solo estamos actualizando el campo oculto
    // en el evento de clic de la lista de productos. El valor correcto ya debería estar establecido
    // en 'hiddenProductName' cuando el formulario se envía.
});
