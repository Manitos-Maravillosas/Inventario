using System.ComponentModel.DataAnnotations;

namespace Sistema_Inventario_Manitos_Maravillosas.Models.Admin
{
    public class Client
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Client name is required.")]
        [StringLength(50, ErrorMessage = "Client name must be between {2} and {1} characters.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Client last name 1 is required.")]
        [StringLength(50, ErrorMessage = "Client last name 1 must be between {2} and {1} characters.", MinimumLength = 2)]
        public string LastName1 { get; set; }

        [StringLength(50, ErrorMessage = "Client last name 2 must be at most {1} characters.")]
        public string LastName2 { get; set; }

        [Required(ErrorMessage = "Client email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(150, ErrorMessage = "Client email must be at most {1} characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Client name is required.")]
        public string PhoneNumber { get; set; }
    }
}
