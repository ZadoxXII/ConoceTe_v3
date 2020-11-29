using ConoceTe.Helpers;
using ConoceTe.WebApp.Models;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConoceTe.Controllers
{
    [Authorize]
    public class CitaController : Controller
    {
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "SY69Fjg4reYO7Fvjm9Z9A9L0eJ0KxJXWUjxclvRu",
            BasePath = "https://conocete-a0251.firebaseio.com/"
        };
        IFirebaseClient client;

        [HttpGet]
        [Authorize(Roles = "Psicologo")]
        public ActionResult CrearServicio()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Psicologo")]
        public ActionResult CrearServicio(Cita cita)
        {
            cita.CitaEstado = "Ci001"; //Estado inicial al crear, disponible para Paciente

            AddCitaFirebase(cita);
            return RedirectToAction("EnlistarServicios", "Cita", new { id = cita.Psicologo.PsicologoID });
        }
        private void AddCitaFirebase(Cita cita)
        {
            client = new FireSharp.FirebaseClient(config);
            var DatosDeCita = cita;
            //Crear codigo y cita
            PushResponse response = client.Push("Cita/", DatosDeCita);
            DatosDeCita.CitaID = response.Result.name;
            //Enviar datos de Cita a DB
            SetResponse setResponse = client.Set("Cita/" + DatosDeCita.CitaID, DatosDeCita);
        }

        [Authorize(Roles = "Psicologo")]
        public ActionResult EnlistarServicios(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Cita/");
            dynamic DatosCita = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var ListaCitasTotal = new List<Cita>();
            var ListaCitasPsicologo = new List<Cita>();
            if (DatosCita != null)
            {
                foreach (var x in DatosCita)
                {
                    ListaCitasTotal.Add(JsonConvert.DeserializeObject<Cita>(((JProperty)x).Value.ToString()));
                }

                foreach (var a in ListaCitasTotal)
                {
                    if(a.CitaFecha < DateTime.Today)
                    {
                        a.CitaEstado = "Ci000";
                        SetResponse responseActualizar = client.Set("Cita/" + a.CitaID + "/CitaEstado/", a.CitaEstado);
                    }

                    if (a.Psicologo.PsicologoID == id)
                    {
                        ListaCitasPsicologo.Add(a);
                    }
                }
            }
            return View(ListaCitasPsicologo);
        }

        [HttpGet]
        [Authorize(Roles = "Psicologo")]
        public ActionResult EliminaCita(string CitaID, string PsicologoID)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("Cita/" + CitaID);
            return RedirectToAction("EnlistarServicios", "Cita", new { id = PsicologoID });
        }

        [HttpGet]
        [Authorize(Roles = "Paciente")]
        public ActionResult MostrarCitas()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Psicologo");
            dynamic CodigosPsicologo = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var ListaCodigosPsi = new List<Usuario>();
            var ListaPsicologo = new List<Usuario>();
            if (CodigosPsicologo != null)
            {
                foreach (var x in CodigosPsicologo)
                {
                    ListaCodigosPsi.Add(JsonConvert.DeserializeObject<Usuario>(((JProperty)x).Value.ToString()));
                }

                ListaPsicologo = AddCodeToPsicologo(ListaCodigosPsi);
            }
            return View(ListaPsicologo);
        }
        public List<Usuario> AddCodeToPsicologo(List<Usuario> ListaCodigosPsi)
        {
            var ListaPsicologo = new List<Usuario>();
            foreach (var x in ListaCodigosPsi)
            {
                var UsuarioJSON = client.Get("Usuario/" + x.UsuarioID);
                Usuario usuario = UsuarioJSON.ResultAs<Usuario>();
                ListaPsicologo.Add(usuario);
            }
            return (ListaPsicologo);
        }

        [HttpGet]
        [Authorize(Roles = "Paciente")]
        public ActionResult SeleccionarCita(string id)
        {
            client = new FireSharp.FirebaseClient(config);

            var UsuarioJSON = client.Get("Usuario/" + id);
            Usuario usuario = UsuarioJSON.ResultAs<Usuario>();

            var ListaCitaPsicologo = new List<Cita>();
            if (usuario != null)
            {
                ListaCitaPsicologo = AddCitaToPsicologo(usuario);
            }
            return View(ListaCitaPsicologo);
        }
        public List<Cita> AddCitaToPsicologo(Usuario usuario)
        {
            FirebaseResponse response = client.Get("Cita");
            dynamic DatosCita = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var ListaCita = new List<Cita>();
            foreach (var x in DatosCita)
            {
                ListaCita.Add(JsonConvert.DeserializeObject<Cita>(((JProperty)x).Value.ToString()));
            }

            var ListaCitaPsicologo = new List<Cita>();
            foreach (var z in ListaCita)
            {
                if (z.CitaFecha >= DateTime.Today)
                {
                    if (usuario.UsuarioID == z.Psicologo.PsicologoID)
                    {
                        ListaCitaPsicologo.Add(
                                 new Cita()
                                 {
                                     CitaID = z.CitaID,
                                     CitaFecha = z.CitaFecha,
                                     CitaPrecio = z.CitaPrecio,
                                     CitaTipo = z.CitaTipo,
                                     CitaEstado = z.CitaEstado,
                                     Psicologo = new Psicologo
                                     {
                                         PsicologoID = usuario.UsuarioID,
                                         Usuario = new Usuario
                                         {
                                             UsuarioNombre = usuario.UsuarioNombre,
                                             UsuarioApellido = usuario.UsuarioApellido,
                                         }
                                     },
                                 }
                        );
                    }
                }
            }
            return (ListaCitaPsicologo);
        }

        [Authorize(Roles = "Paciente")]
        public ActionResult SolicitarCita(string CitaID, string PacienteID)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("Cita/" + CitaID + "/Paciente/PacienteID/", PacienteID);
            SetResponse responseN2 = client.Set("Cita/" + CitaID + "/CitaEstado/", "Ci002");
            return RedirectToAction("MostrarCitas", "Cita");
        }
    }
}