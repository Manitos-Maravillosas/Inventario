
document.addEventListener('DOMContentLoaded', function () {
    var allowDeleverySwitch = document.getElementById('allowDeleverySwitch');

    if (allowDeleverySwitch != null) {


        
        allowDeleverySwitch.addEventListener('change', function () {
            var typeDeleverySelect = document.getElementById('typeDeleverySelect');

            if (allowDeleverySwitch.checked) {
                typeDeleverySelect.disabled = false;

                // Llamando directamente a GetTypeDeliveries
                GetTypeDeliveries().then(jsonData => {
                    // Limpiar opciones existentes
                    typeDeleverySelect.innerHTML = '<option disabled>Seleccione un método de Envío</option>';

                    if (jsonData.length > 0) {
                        // Añadir nuevas opciones
                        console.log(jsonData);
                        jsonData.forEach(function (deliveryType) {
                            var option = new Option(deliveryType.name, deliveryType.id);
                            typeDeleverySelect.add(option);
                        });
                    }
                }).catch(error => {
                    console.error('Error:', error);
                });
            } else {
                typeDeleverySelect.disabled = true;
                typeDeleverySelect.innerHTML = '<option disabled>Seleccione un método de Envío</option>';
            }
        });


        
            //load data to the type delevery

    }

 });


import { GetTypeDeliveries } from './facturationService.js';