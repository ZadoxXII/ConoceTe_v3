using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConoceTe.WebApp.Models
{
    public class Cita
    {
        public string CitaID { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.DateTime)]
        public DateTime CitaFecha { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public double CitaPrecio { get; set; }

        [DataType(DataType.Url)]
        public string CitaEnlace { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string CitaTipo { get; set; }

        public string CitaEstado { get; set; }

        public Psicologo Psicologo { get; set; }

        public List<Paciente> Pacientes { get; set; }
    }
}