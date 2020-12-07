using ConoceTe.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConoceTe.Models
{
    public class Pago
    {
        public string PagoID { get; set; }

        public double PagoComprobante { get; set; }

        public double PagoMonto { get; set; }

        public DateTime PagoFecha { get; set; }


        public Paciente Paciente { get; set; }

        public Psicologo Psicologo { get; set; }

        public Cita Cita { get; set; }
    }
}