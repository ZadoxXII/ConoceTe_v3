using ConoceTe.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConoceTe.WebApp.DBBorrableFolder
{
    public class UsuarioRepositorio
    {
        public List<Usuario> ListandoUsuarios()
        {
            return new List<Usuario>()
            {
                new Usuario()
                {
                    UsuarioNombre = "Al Code",
                    UsuarioApellido = "Falacio Hupey",
                    UsuarioRol = "Psicologo"
                },

                new Usuario()
                {
                    UsuarioNombre = "Divi Flogi",
                        UsuarioApellido = "Tay Dela",
                        UsuarioRol = "Psicologo"
                }
            };
        }
    }
}
