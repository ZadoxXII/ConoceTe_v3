using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConoceTe.WebApp.Models
{
    public class Psicologo
    {
        public string PsicologoID { get; set; }

        [Required]
        public string PsicologoDNI { get; set; }

        [Required]
        public string PsicologoEspecialidad { get; set; }

        [Required]
        public string PsicologoUniversidad { get; set; }

        public string PsicologoDescripcion { get; set; }

        public Usuario Usuario { get; set; }

        public List<Cita> Citas { get; set; }
    }
}