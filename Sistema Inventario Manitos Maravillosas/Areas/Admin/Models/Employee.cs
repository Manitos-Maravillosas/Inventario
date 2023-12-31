using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Inventario_Manitos_Maravillosas.Models.Admin;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models
{
    public class Employee
    {
        [Key]
        [Required(ErrorMessage = "Se requiere la identificación del empleado.")]
        [StringLength(60, ErrorMessage = "La identificación del empleado debe tener más de 1 letra.")]
        public string IdEmployee { get; set; }

        [Required(ErrorMessage = "Se requiere el nombre del empleado.")]
        [StringLength(50, ErrorMessage = "El nombre del empleado debe tener más de 1 letra.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Se requiere el primer apellido del empleado.")]
        [StringLength(50, ErrorMessage = "El primer apellido del empleado debe tener más de 1 letra.", MinimumLength = 2)]
        public string LastName1 { get; set; }

        [StringLength(50, ErrorMessage = "El segundo apellido del empleado debe tener más de 1 letra.")]
        public string LastName2 { get; set; }

        [Required(ErrorMessage = "Se requiere el puesto del empleado.")]
        [StringLength(50, ErrorMessage = "El puesto del empleado debe tener más de 1 letra.")]
        public string Position { get; set; }

        [Required(ErrorMessage = "El número de contacto del empleado es requerido.")]
        [StringLength(60, ErrorMessage = "El número de contacto del empleado debe tener más de 1 letra.")]
        public string PhoneNumber { get; set; }

        // Clave foránea y propiedad de navegación para Business
        [Required(ErrorMessage = "Se requiere el ID del negocio.")]
        public int IdBusiness { get; set; }
        public virtual Business Business { get; set; }

        // Clave foránea y propiedad de navegación para User
        [Required(ErrorMessage = "Se requiere el nombre de usuario.")]
        [StringLength(100, ErrorMessage = "El nombre de usuario debe tener más de 1 letra.")]
        public string Username { get; set; }
        public virtual User User { get; set; }

        public string BusinessName { get; set; }

        public int SelectedBusiness { get; set; }
        public List<SelectListItem> BusinessList { get; set; }

       
            
    }
}
