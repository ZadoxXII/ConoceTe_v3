using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConoceTe.WebApp.Models
{
    public class Paciente
    {
        public string PacienteID { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaNaci { get; set; }

        public Usuario Usuario { get; set; }

        public List<Cita> Citas { get; set; }
    }
}