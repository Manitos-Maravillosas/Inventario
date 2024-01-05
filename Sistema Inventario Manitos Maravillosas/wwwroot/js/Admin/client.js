
document.addEventListener('DOMContentLoaded', function () {

    // Modal Address
    var assignAddressBtn = document.getElementById('assignAddress');
    var modalAddress = new bootstrap.Modal(document.getElementById('modalAddress'));

    assignAddressBtn.addEventListener('click', function () {
        modalAddress.show();
    });
    document.getElementById('departmentSelect').addEventListener('change', function () {
        var departmentName = this.value;
        if (departmentName) {
            fetch('/Admin/Client/GetCitiesByDepartment?departmentName=' + encodeURIComponent(departmentName))
                .then(response => response.json())
                .then(data => {
                    var citySelect = document.getElementById('citySelect');
                    citySelect.innerHTML = '<option value="">Selecciona una Ciudad</option>';
                    data.forEach(function (city) {
                        citySelect.options[citySelect.options.length] = new Option(city, city);
                    });
                    citySelect.disabled = false;
                })
                .catch(error => {
                    console.error('Error al obtener las ciudades:', error);
                });
        }
    });   

});

