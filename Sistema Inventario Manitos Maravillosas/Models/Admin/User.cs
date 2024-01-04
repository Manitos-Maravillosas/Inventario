using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Models.Admin
{
    public class User
    {
        [Key]
        [Required(ErrorMessage = "Se requiere el nombre de usuario.")]
        [StringLength(100, ErrorMessage = "El nombre de usuario debe tener más de 1 letra.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Se requiere el correo electrónico.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico es inválido.")]
        [StringLength(150, ErrorMessage = "El correo electrónico debe tener más de 1 letra.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Se requiere la contraseña.")]
        [StringLength(256, ErrorMessage = "La contraseña debe tener más de 1 letra.")]
        public string Password { get; set; }
    }
}
