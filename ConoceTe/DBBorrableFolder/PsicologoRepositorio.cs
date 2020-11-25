using ConoceTe.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConoceTe.WebApp.DBBorrableFolder
{
    public class PsicologoRepositorio
    {
        public List<Psicologo> ListandoPsicogos()
        {
            return new List<Psicologo>()
            {
                new Psicologo()
                {
                    PsicologoID = "1001",
                    Usuario = new Usuario
                    {
                        UsuarioNombre = "Al Code",
                        UsuarioApellido = "Falacio Hupey",
                        UsuarioRol = "Psicologo"
                    },

                    PsicologoDescripcion = "Psicologo experto en Terapia familiar con 5 años de experiencia",
                    PsicologoEspecialidad = "Terapia Familiar",
                    PsicologoUniversidad = "UTP",

                    Citas = new List<Cita>(){
                    
                        new Cita()
                        {
                            CitaFecha = new DateTime(2020, 11, 20, 20, 45, 0),
                            CitaPrecio = 60,
                            CitaTipo = "Privada",
                            CitaEstado = "Ci001"
                        },

                        new Cita()
                        {
                            CitaFecha = new DateTime(2020, 11, 19, 19, 45, 0),
                            CitaPrecio = 60,
                            CitaTipo = "Privada",
                            CitaEstado = "Ci002"
                        },
                        new Cita()
                        {
                            CitaFecha = new DateTime(2020, 11, 10, 18, 0, 0),
                            CitaPrecio = 60,
                            CitaTipo = "Privada",
                            CitaEstado = "Ci003"
                        }
                    }
                },

                new Psicologo()
                {
                    PsicologoID = "1002",
                    Usuario = new Usuario
                    {
                        UsuarioNombre = "Divi Flogi",
                        UsuarioApellido = "Tay Dela",
                        UsuarioRol = "Psicologo"
                    },

                    PsicologoDescripcion = "Psicologo con postgrado en la universidad de AEO en terapia de parejas",
                    PsicologoEspecialidad = "Terapia de parejas",
                    PsicologoUniversidad = "Universidad de AEO",

                    Citas = new List<Cita>(){
                         new Cita()
                        {
                            CitaFecha = new DateTime(2020, 11, 24, 21, 30, 0),
                            CitaPrecio = 120,
                            CitaTipo = "Privada",
                            CitaEstado = "Ci001"
                        },
                        new Cita()
                        {
                            CitaFecha = new DateTime(2020, 11, 25, 22, 0, 0),
                            CitaPrecio = 120,
                            CitaTipo = "Privada",
                            CitaEstado = "Ci002"
                        },
                        new Cita()
                        {
                            CitaFecha = new DateTime(2020, 11, 26, 20, 0, 0),
                            CitaPrecio = 120,
                            CitaTipo = "Privada",
                            CitaEstado = "Ci001"
                        },
                        new Cita()
                        {
                            CitaFecha = new DateTime(2020, 11, 27, 10, 30, 0),
                            CitaPrecio = 120,
                            CitaTipo = "Privada",
                            CitaEstado = "Ci002"
                        },
                    },
                }
            };
        }
    }
}