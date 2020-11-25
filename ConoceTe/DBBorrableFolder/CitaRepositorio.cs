using ConoceTe.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConoceTe.WebApp.DBBorrableFolder
{
    public class CitaRepositorio
    {
        public List<Cita> ListandoCitas()
        {
            return new List<Cita>()
            {
                new Cita()
                {
                    CitaFecha = new DateTime(2020, 11, 20),
                    CitaPrecio = 60,
                    CitaTipo = "Privada",
                    CitaEstado = "Ci001",
                },
            };
        }
    }
}
