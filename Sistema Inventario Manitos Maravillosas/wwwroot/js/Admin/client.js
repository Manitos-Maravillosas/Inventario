document.addEventListener('DOMContentLoaded', function () {
    // Variables para almacenar las selecciones actuales
    var selectedDepartment = '@Model.DepartmentName';
    var selectedCity = '@Model.CityName';

    var modalAddress = new bootstrap.Modal(document.getElementById('modalAddress'));
    var departmentSelect = document.getElementById('departmentSelect');
    var citySelect = document.getElementById('citySelect');
    var addressInput = document.getElementById('additionalInfoInput');

    cargarDepartamentos();
    if (selectedDepartment) {
        // En lugar de usar option.selected, seleccionamos la opción por su índice
        for (var i = 0; i < departmentSelect.options.length; i++) {
            if (departmentSelect.options[i].value === selectedDepartment) {
                departmentSelect.selectedIndex = i;
                break;  // Salir del bucle una vez que se haya encontrado la coincidencia
            }
        }
        cargarCiudades(selectedDepartment);
    }
    
    // Evento para abrir el modal y cargar los departamentos
    document.getElementById('assignAddress').addEventListener('click', function () {
        cargarDepartamentos();
        modalAddress.show();
    });

    // Evento para manejar el cambio en el departamento
    departmentSelect.addEventListener('change', function () {
        selectedDepartment = this.value;
        cargarCiudades(selectedDepartment);
    });

    // Evento para manejar el cambio en la ciudad
    citySelect.addEventListener('change', function () {
        selectedCity = this.value;
        addressInput.disabled = selectedCity === '';
        addressInput.placeholder = selectedCity ? "Dirección Exacta" : "Primero seleccione un municipio";
    });

    // Función para cargar departamentos
    function cargarDepartamentos() {
        fetch('/Admin/Client/GetDepartmentNames')
            .then(response => response.json())
            .then(data => {
                departmentSelect.innerHTML = '<option value="">Selecciona un Departamento</option>';
                data.forEach(function (department) {
                    var option = new Option(department, department);
                    option.selected = department === selectedDepartment;
                    departmentSelect.add(option);
                });
                if (selectedDepartment) {
                    cargarCiudades(selectedDepartment);
                }
            })
            .catch(error => {
                console.error('Error al obtener los departamentos:', error);
            });
    }

    // Función para cargar ciudades
    function cargarCiudades(departmentName) {
        fetch('/Admin/Client/GetCitiesByDepartment?departmentName=' + encodeURIComponent(departmentName))
            .then(response => response.json())
            .then(data => {
                citySelect.innerHTML = '<option value="">Selecciona una Ciudad</option>';
                data.forEach(function (city) {
                    var option = new Option(city, city);
                    if (city === selectedCity) {
                        option.selected = true; // Selecciona la ciudad que corresponde al cliente
                    }
                    citySelect.add(option);
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

        if (!department || !city || !address) {
            
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
