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

    // Seleccionar producto
    var listProduct = document.getElementById('listProduct');
    if (listProduct) {
        var elements = listProduct.getElementsByTagName('li');
        for (var i = 0; i < elements.length; i++) {
            elements[i].addEventListener('click', function () {
                
                var name = this.getAttribute('data-name');

                document.getElementById('productNameInput').value =  name;

                modalProduct.hide();
            });
        }
    }

});
