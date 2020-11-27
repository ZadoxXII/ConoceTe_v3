using ConoceTe.WebApp.DBBorrableFolder;
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

        //Objetos de prueba para mostrar - BORRABLE
        private PsicologoRepositorio _psicologoRepositorio;
        public CitaController()
        {
            _psicologoRepositorio = new PsicologoRepositorio();
        }

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
            return RedirectToAction("EnlistarMisCitas", "Cita");
        }
        private void AddCitaFirebase(Cita cita)
        {
            client = new FireSharp.FirebaseClient(config);
            var DatosDeCita = cita;
            PushResponse response = client.Push("Cita/", DatosDeCita);
            DatosDeCita.CitaID = response.Result.name;
            //DatosDeCita.Psicologo.PsicologoID = DatosDeCita.CitaID;
            SetResponse setResponse = client.Set("Cita/" + DatosDeCita.CitaID, DatosDeCita);
        }

        [HttpGet]
        [Authorize(Roles = "Psicologo")]
        public ActionResult EnlistarServicios()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Cita");
            dynamic DatosCita = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var ListaCita = new List<Cita>();
            foreach(var x in DatosCita)
            {
                ListaCita.Add(JsonConvert.DeserializeObject<Cita>(((JProperty)x).Value.ToString()));
            }
            return View(ListaCita);
        }

        [HttpGet]
        [Authorize(Roles = "Paciente")]
        public ActionResult MostrarCitas()
        {
            var model = _psicologoRepositorio.ListandoPsicogos();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Paciente")]
        public ActionResult SeleccionarCita(string id)
        {
            var Lista_model = _psicologoRepositorio.ListandoPsicogos();
            var model = Lista_model[0];
            foreach (var psico in Lista_model)
            {
                if (id == psico.PsicologoID)
                {
                    model = psico;
                }
            }
            return View(model);
        }
    }
}