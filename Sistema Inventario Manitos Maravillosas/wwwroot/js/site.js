// script.js
document.getElementById("botonDesplegable").addEventListener("click", function () {
    var opciones = document.getElementById("opcionesMoneda");
    if (opciones.style.display === "block") {
        opciones.style.display = "none";
    } else {
        opciones.style.display = "block";
    }
});
