
document.addEventListener('DOMContentLoaded', function () {

    var typePaymentSelect = document.getElementById('typePaymentSelect');
    var allowMixPaymentDiv = document.getElementById('allowMixPaymentDiv');
    var allowMixPaymentSwitch = document.getElementById('allowMixPaymentSwitch');
    var bankAccountId = document.getElementById('bankAccountId');

    var divRadioCoin = document.getElementById('divRadioCoin');
    var radiosCoins = document.querySelectorAll('input[type="radio"][name="radioCoin"]');
    var divCoinAmountPaid = document.getElementById('divCoinAmountPaid');

    
    if (typePaymentSelect != null) {
        typePaymentSelect.addEventListener('change', function () {

            if (typePaymentSelect.value == "1" && this.options[this.selectedIndex].text == "Efectivo") {
                allowMixPaymentDiv.classList.remove('d-none');
                bankAccountId.classList.add('d-none');

                SwitchChange()

            } else if (typePaymentSelect.value == "2") { //bank transfer
                bankAccountId.classList.remove('d-none');
                allowMixPaymentDiv.classList.add('d-none');

                allowMixPaymentSwitch.checked
                SwitchChange()
            } else if (typePaymentSelect.value == "3") {
                POSDisplay()
            }
        });
    }

    if (allowMixPaymentSwitch !=null) {
        allowMixPaymentSwitch.addEventListener('change', function () {
            SwitchChange()
            
        });
    }

    function SwitchChange() {
        if (allowMixPaymentSwitch.checked) {
            divCoinAmountPaid.classList.remove('d-none');
            divRadioCoin.classList.add('d-none');
        } else {
            divCoinAmountPaid.classList.add('d-none');
            divRadioCoin.classList.remove('d-none');
        }
    }


    function POSDisplay() {
        allowMixPaymentDiv.classList.add('d-none');
        bankAccountId.classList.add('d-none');
        divCoinAmountPaid.classList.add('d-none');
        divRadioCoin.classList.add('d-none');
    }

    //Reload page
    

});
