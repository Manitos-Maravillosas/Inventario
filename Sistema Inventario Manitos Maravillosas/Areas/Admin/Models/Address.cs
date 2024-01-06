using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAddress { get; set; }

        [Required]
        [StringLength(150)]
        public string Signs { get; set; }

        [ForeignKey("City")]
        public int IdCity { get; set; }        
    }
}
