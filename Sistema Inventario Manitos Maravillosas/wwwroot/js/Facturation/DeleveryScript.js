
document.addEventListener('DOMContentLoaded', function () {
    var allowDeleverySwitch = document.getElementById('allowDeleverySwitch');
    var typeDeliverySelect = document.getElementById('typeDeliverySelect');
    var companyTransSelect = document.getElementById('companyTransSelect');

    var divForDepartamental = document.getElementById('divForDepartamental');
    var dvInChargeAditionalCost = document.getElementById('dvInChargeAditionalCost');

    var inputDateAprox = document.getElementById('inputDateAprox');
    var inputInternalCost = document.getElementById('inputInternalCost');

    if (allowDeleverySwitch != null) {        
        allowDeleverySwitch.addEventListener('change', function () {
           

            if (allowDeleverySwitch.checked) {
                typeDeliverySelect.disabled = false;  
                inputDateAprox.disabled = false;
                inputInternalCost.disabled = false;
                
            } else {
                restartDeleveryOff()
            }
        });      
    }

    if (typeDeliverySelect != null) {
        typeDeliverySelect.addEventListener('change', function () {

            if (this.value == '2') {
                divForDepartamental.classList.remove('d-none');
            } else {
                divForDepartamental.classList.add('d-none');
            }
        });
    }

    if (companyTransSelect != null) {
        companyTransSelect.addEventListener('change', function () {
            if (this.value == '2') {
                dvInChargeAditionalCost.classList.remove('d-none');
            }else {
                dvInChargeAditionalCost.classList.add('d-none');
            }
        });
    }
    

    function restartDeleveryOff() {
        allowDeleverySwitch.checked = false;
        typeDeliverySelect.disabled = true;
        inputDateAprox.disabled = true;
        inputInternalCost.disabled = true;
        divForDepartamental.classList.add('d-none');
    }

    //Reload page
    if (allowDeleverySwitch.checked) {
        typeDeliverySelect.disabled = false;
        inputDateAprox.disabled = false;
        inputInternalCost.disabled = false;

    }


    if (typeDeliverySelect.value == '2') {
        divForDepartamental.classList.remove('d-none');

    } else {
        divForDepartamental.classList.add('d-none');
    }

    if (companyTransSelect.value == '2') {
        dvInChargeAditionalCost.classList.remove('d-none');
    } else {
        dvInChargeAditionalCost.classList.add('d-none');
    }

    var radios = document.querySelectorAll('input[type="radio"][name="radioCharge"]');

    radios.forEach(function (radio) {
        radio.addEventListener('change', function () {
            console.log('radio changed');
            console.log('radio.value:', radio.value);
            radio.setAttribute('checked', true);
            radios.forEach(function (otherRadio) {
                if (otherRadio !== radio) {
                    otherRadio.setAttribute('checked', false);
                    otherRadio.checked = false;
                }
            });
        });
    });

 });



import { GetTypeDeliveries, GetCompanyTrans } from './facturationService.js';