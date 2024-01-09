using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models
{
    public class Client
    {
        [Required(ErrorMessage = "Se requiere la identificación del cliente.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Se requiere el nombre del cliente..")]
        [StringLength(50, ErrorMessage = "El nombre del cliente debe tener más de 1 letra.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Se requiere el primer apellido del cliente..")]
        [StringLength(50, ErrorMessage = "El primer apellido del cliente debe tener más de 1 letra.", MinimumLength = 2)]
        public string LastName1 { get; set; }

        [StringLength(50, ErrorMessage = "El segundo apellido del cliente debe tener más de 1 letra.")]
        public string LastName2 { get; set; }
        
        [Required(ErrorMessage = "El número de contacto del cliente es requerido.")]
        public string PhoneNumber { get; set; }           

        public int IdAddress { get; set; }
        [Required(ErrorMessage = "Debe elegir un departamento.")]
        public string DepartmentName { get; set; }
        [Required(ErrorMessage = "Debe elegir un municipio.")]
        public string CityName { get; set; }
        [Required(ErrorMessage = "Debe ingresar la dirección exacta.")]
        public string Signs { get; set; }        
    }
}
