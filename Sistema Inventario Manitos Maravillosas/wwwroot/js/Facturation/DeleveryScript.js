
document.addEventListener('DOMContentLoaded', function () {
    var allowDeleverySwitch = document.getElementById('allowDeleverySwitch');
    var typeDeliverySelect = document.getElementById('typeDeliverySelect');
    var companyTransSelect = document.getElementById('companyTransSelect');

    var divForDepartamental = document.getElementById('divForDepartamental');
    var dvInChargeAditionalCost = document.getElementById('dvInChargeAditionalCost');
    if (allowDeleverySwitch != null) {


        
        allowDeleverySwitch.addEventListener('change', function () {
           

            if (allowDeleverySwitch.checked) {
                typeDeliverySelect.disabled = false;

                // Llamando directamente a GetTypeDeliveries
                GetTypeDeliveries().then(jsonData => {
                    // Limpiar opciones existentes
                    typeDeliverySelect.innerHTML = '<option disabled selected>Seleccione un método de Envío</option>';

                    if (jsonData.length > 0) {
                        jsonData.forEach(function (deliveryType) {
                            var option = new Option(deliveryType.name, deliveryType.id);
                            typeDeliverySelect.add(option);
                        });
                    }
                }).catch(error => {
                    console.error('Error:', error);
                });
            } else {
                restartDeleveryOff()
            }
        });      
    }

    if (typeDeliverySelect != null) {
        typeDeliverySelect.addEventListener('change', function () {

            if (this.value == '2') {
                GetCompanyTrans().then(jsonData => {
                    //load companyTrans                    
                    companyTransSelect.innerHTML = '<option disabled selected>Seleccione una Compañia de Transporte</option>';

                    if (jsonData.length > 0) {
                        jsonData.forEach(function (companyTrans) {
                            console.log(companyTrans);
                            var option = new Option(companyTrans.name, companyTrans.idCompanyTrans);
                            companyTransSelect.add(option);
                        });
                    }


                }).catch(error => {
                    console.error('Error:', error);
                });

                divForDepartamental.classList.remove('d-none');

            } else {
                divForDepartamental.classList.add('d-none');
            }
        });
    }

    if (companyTransSelect != null) {
        companyTransSelect.addEventListener('change', function () {
            console.log(this.value);
            if (this.value == '2') {
                dvInChargeAditionalCost.classList.remove('d-none');
            }else {
                dvInChargeAditionalCost.classList.add('d-none');
            }
        });
    }
    

    function restartDeleveryOff(){
        allowDeleverySwitch.checked = false;
        typeDeliverySelect.disabled = true;
        typeDeliverySelect.innerHTML = '<option disabled selected>Seleccione un método de Envío</option>';
        divForDepartamental.classList.add('d-none');
    }

 });



import { GetTypeDeliveries, GetCompanyTrans } from './facturationService.js';