document.addEventListener('DOMContentLoaded', function () {
    var selectedDepartment = document.getElementById('departmentInfo').dataset.department;
    var selectedCity = document.getElementById('cityInfo').dataset.city;
    var selectedAddress = document.getElementById('addressInfo').dataset.address;
    var action = document.getElementById('actionInfo').dataset.action;

    var modalAddress = new bootstrap.Modal(document.getElementById('modalAddress'));
    var departmentSelect = document.getElementById('departmentSelect');
    var citySelect = document.getElementById('citySelect');
    var addressInput = document.getElementById('additionalInfoInput');

    cargarDepartamentos();
    // Inicializar deshabilitados para el modo "Crear"
    if (action === 'Create') {
        citySelect.disabled = true;
        addressInput.disabled = true;
    }
    if (action === 'Edit') {
        document.getElementById('displayDepartment').textContent = selectedDepartment;
        document.getElementById('displayCity').textContent = selectedCity;
        document.getElementById('displayAddress').textContent = selectedAddress;

        // Permitir la edición del address sin cambiar departamento o ciudad
        addressInput.disabled = false;
    }

    document.getElementById('assignAddress').addEventListener('click', function () {
        cargarDepartamentos();
        modalAddress.show();
    });

    departmentSelect.addEventListener('change', function () {
        selectedDepartment = this.value;
        cargarCiudades(selectedDepartment);
        // Habilita la entrada de la dirección después de seleccionar un departamento
        addressInput.disabled = false;
    });

    citySelect.addEventListener('change', function () {
        selectedCity = this.value;
        addressInput.placeholder = selectedCity ? "Dirección Exacta" : "Primero seleccione un municipio";
    });

    function cargarDepartamentos() {
        fetch('/Admin/Client/GetDepartmentNames')
            .then(response => response.json())
            .then(data => {
                departmentSelect.innerHTML = '<option value="">Selecciona un Departamento</option>';
                data.forEach(function (department) {
                    var option = new Option(department, department);
                    departmentSelect.add(option);
                    if (department === selectedDepartment && action === 'Edit') {
                        option.selected = true;
                    }
                });
                if (selectedDepartment && action === 'Edit') {
                    cargarCiudades(selectedDepartment);
                }
            })
            .catch(error => {
                console.error('Error al obtener los departamentos:', error);
            });
    }

    function cargarCiudades(departmentName) {
        fetch('/Admin/Client/GetCitiesByDepartment?departmentName=' + encodeURIComponent(departmentName))
            .then(response => response.json())
            .then(data => {
                citySelect.innerHTML = '<option value="">Selecciona una Ciudad</option>';
                data.forEach(function (city) {
                    var option = new Option(city, city);
                    citySelect.add(option);
                    if (city === selectedCity && action === 'Edit') {
                        option.selected = true;
                    }
                });
                citySelect.disabled = false;
            })
            .catch(error => {
                console.error('Error al obtener las ciudades:', error);
            });
    }

    window.saveAddress = function () {
        var department = departmentSelect.value;
        var city = citySelect.value;
        var address = addressInput.value;

        if (action === 'Create' && (!department || !city || !address)) {
            // En modo 'Crear', no permitir guardar sin todos los campos
            return;
        }

        document.getElementById('hiddenDepartmentField').value = department;
        document.getElementById('hiddenCityField').value = city;
        document.getElementById('hiddenAddressField').value = address;

        document.getElementById('displayDepartment').textContent = department;
        document.getElementById('displayCity').textContent = city;
        document.getElementById('displayAddress').textContent = address;

        modalAddress.hide();
    };
});
