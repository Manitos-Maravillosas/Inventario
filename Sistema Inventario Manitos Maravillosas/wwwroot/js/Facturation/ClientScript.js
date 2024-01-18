

document.addEventListener('DOMContentLoaded', function () {


    //-------------------------------------------------------------------------------------//
    //                                  Client Modal                                       //
    //-------------------------------------------------------------------------------------//
    var assignClient = document.querySelector('#assignClient');

    // modals
    var clientModal = new bootstrap.Modal(document.getElementById('modalClient'));
    var newClient = new bootstrap.Modal(document.getElementById('addClient'));
    var deliveryModal = new bootstrap.Modal(document.getElementById('modalDelivery'));
    
    assignClient.addEventListener('click', function () {        
        clientModal.show();
    });

    var openNewClient = document.querySelector('#openNewClient');

    if (openNewClient) {
        openNewClient.addEventListener('click', function () {

            clientModal.hide(); openNewClient
            newClient.show();
        });
    }

    //filter identification
    document.getElementById('identificationClientFilter').addEventListener('input', function () {
        var inputVal = this.value;
        var listItems = document.querySelectorAll('#listClient .list-group-item');
        if (listItems.length > 0) {
            listItems.forEach(function (item) {
                var clientId = item.getAttribute('data-id');
                if (clientId.includes(inputVal)) {
                    item.style.display = ''; // Show the item if it matches
                } else {
                    item.style.display = 'none'; // Hide the item if it doesn't match
                }
            });
        }
        
    });

    //-----------------------------------------------------select client
    var listClient = document.getElementById('listClient');
    if (listClient) {

        var elements = listClient.getElementsByTagName('li');
        for (var i = 0; i < elements.length; i++) {
            elements[i].addEventListener('click', function () {
                //for (let j = 0; j < elements.length; j++) {
                //    elements[j].classList.remove('active');
                //}

                //// Add 'active' class to the clicked element
                //this.classList.add('active');

                var id = this.getAttribute('data-id');
                var name = this.getAttribute('data-name');
                var idAddress = this.getAttribute('data-idaddress');

                document.getElementById('clientDataInput').value = id + ' - '+name;
                document.getElementById('idAddressClient').value = idAddress;

                assignClientToBill(id);

                clientModal.hide();
            });
        }       
    }
    

});
import { assignClientToBill } from './facturationService.js';