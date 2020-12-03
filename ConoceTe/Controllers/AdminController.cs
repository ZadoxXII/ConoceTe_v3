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
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "SY69Fjg4reYO7Fvjm9Z9A9L0eJ0KxJXWUjxclvRu",
            BasePath = "https://conocete-a0251.firebaseio.com/"
        };
        IFirebaseClient client;

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AccesoPsico()
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
                if (usuario.UsuarioEstado == "Ps00")
                {
                    ListaPsicologo.Add(usuario);
                }
            }
            return (ListaPsicologo);
        }

        public ActionResult Aceptar(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("Usuario/" + id + "/UsuarioEstado/", "Ps01");
            return RedirectToAction("AccesoPsicologo");
        }

        public ActionResult Eliminar(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("Usuario/" + id);
            FirebaseResponse responseN2 = client.Delete("Psicologo/" + id);
            return RedirectToAction("AccesoPsicologo");
        }
    }
}
