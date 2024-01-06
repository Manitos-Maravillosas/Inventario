document.addEventListener('DOMContentLoaded', function () {   
    var selectedDepartment = document.getElementById('departmentInfo').dataset.department;
    var selectedCity = document.getElementById('cityInfo').dataset.city;    
    var selectedAddress = document.getElementById('addressInfo').dataset.address;

    var modalAddress = new bootstrap.Modal(document.getElementById('modalAddress'));
    var departmentSelect = document.getElementById('departmentSelect');
    var citySelect = document.getElementById('citySelect');
    var addressInput = document.getElementById('additionalInfoInput');

    document.getElementById('displayDepartment').textContent = selectedDepartment;
    document.getElementById('displayCity').textContent = selectedCity;
    document.getElementById('displayAddress').textContent = selectedAddress;

    cargarDepartamentos();
    if (selectedDepartment) {       
        for (var i = 0; i < departmentSelect.options.length; i++) {
            if (departmentSelect.options[i].value === selectedDepartment) {
                departmentSelect.selectedIndex = i;
                break;  
            }
        }
        cargarCiudades(selectedDepartment);
    }
    
    document.getElementById('assignAddress').addEventListener('click', function () {
        cargarDepartamentos();
        modalAddress.show();
    });

    departmentSelect.addEventListener('change', function () {
        selectedDepartment = this.value;
        cargarCiudades(selectedDepartment);
    });

    citySelect.addEventListener('change', function () {
        selectedCity = this.value;
        addressInput.disabled = selectedCity === '';
        addressInput.placeholder = selectedCity ? "Dirección Exacta" : "Primero seleccione un municipio";
    });

    function cargarDepartamentos() {
        fetch('/Admin/Client/GetDepartmentNames')
            .then(response => response.json())
            .then(data => {
                departmentSelect.innerHTML = '<option value="">Selecciona un Departamento</option>';
                data.forEach(function (department) {
                    var option = new Option(department, department);
                    if (department === selectedDepartment) {
                        option.selected = true;
                    }
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
    function cargarCiudades(departmentName) {
        fetch('/Admin/Client/GetCitiesByDepartment?departmentName=' + encodeURIComponent(departmentName))
            .then(response => response.json())
            .then(data => {
                citySelect.innerHTML = '<option value="">Selecciona una Ciudad</option>';
                data.forEach(function (city) {
                    var option = new Option(city, city);
                    if (city === selectedCity) {
                        option.selected = true;
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
