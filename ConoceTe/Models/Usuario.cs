using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConoceTe.WebApp.Models
{
    public class Usuario
    {
        public string UsuarioID { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(25, ErrorMessage = "El campo {0} tiene un máximo de {1} caracteres")]
        public string UsuarioNombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(25, ErrorMessage = "El campo {0} tiene un máximo de {1} caracteres")]
        public string UsuarioApellido { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(25, ErrorMessage = "El campo {0} tiene un máximo de {1} caracteres")]
        [DataType(DataType.EmailAddress)]
        public string UsuarioEmail { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(25, ErrorMessage = "El campo {0} tiene un máximo de {1} caracteres")]
        [DataType(DataType.Password)]
        public string UsuarioContra { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(25, ErrorMessage = "El campo {0} tiene un máximo de {1} caracteres")]
        [DataType(DataType.Password)]
        [Compare("UsuarioContra", ErrorMessage = "Las contraseñas no concuerdan")]
        public string UContraConfirmar { get; set; }

        public string UsuarioPhone { get; set; }
        public string UsuarioRol { get; set; }
        public string UsuarioEstado { get; set; }
    }
}