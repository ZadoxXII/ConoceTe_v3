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
using ConoceTe.Models;
using SecurionPay;
using SecurionPay.Request;
using SecurionPay.Response;
using SecurionPay.Exception;
using SecurionPay.Enums;
using System.Threading.Tasks;

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
            string Estado = VerificarEstado(cita.Psicologo.PsicologoID);
            if (Estado != "Ps00")
            {
                cita.CitaEstado = "Ci001"; //Estado inicial al crear, disponible para Paciente

                AddCitaFirebase(cita);
                return RedirectToAction("EnlistarServicios", "Cita", new { id = cita.Psicologo.PsicologoID });
            }
            else
            {
                return View();
            }
        }
        private string VerificarEstado(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            var UsuarioJSON = client.Get("Usuario/" + id);
            Usuario usuario = UsuarioJSON.ResultAs<Usuario>();
            return usuario.UsuarioEstado;
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
        public ActionResult EnlistarServicios()
        {
            string id = SessionHelper.GetNameIdentifier(User);
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
                    ActualizarEstadoToCita(a.CitaFecha, a.CitaID);

                    if (a.Psicologo.PsicologoID == id)
                    {
                        ListaCitasPsicologo.Add(a);
                    }
                }
            }
            return View(ListaCitasPsicologo);
        }

        private void ActualizarEstadoToCita (DateTime CitaFecha, string CitaID){
            if (CitaFecha < DateTime.Today)
            {
                string CitaEstadoActual = "Ci000";
                SetResponse responseActualizar = client.Set("Cita/" + CitaID + "/CitaEstado/", CitaEstadoActual);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Psicologo")]
        public ActionResult EliminarServicio(string CitaID, string PsicologoID)
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
        public ActionResult SolicitarCita(string CitaID)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("Cita/" + CitaID + "/Paciente/PacienteID/", SessionHelper.GetNameIdentifier(User));
            SetResponse responseN2 = client.Set("Cita/" + CitaID + "/CitaEstado/", "Ci002");
            return RedirectToAction("AceptadoPago", "Cita");
        }

        [HttpGet]
        public ViewResult PagarCita(string CitaID)
        {
            HttpCookie CookCitaID = new HttpCookie("IDCita", CitaID);
            client = new FireSharp.FirebaseClient(config);
            var CitaJSON = client.Get("Cita/" + CookCitaID.Value);
            Cita cita = CitaJSON.ResultAs<Cita>();
            HttpCookie CookPsicologoID = new HttpCookie("IDPsicologo", cita.Psicologo.PsicologoID);
            ViewBag.CitaPrecio = cita.CitaPrecio;

            ControllerContext.HttpContext.Response.SetCookie(CookCitaID);
            ControllerContext.HttpContext.Response.SetCookie(CookPsicologoID);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Paciente")]
        [Obsolete]
        public async Task<ActionResult> PagarCita(Tarjeta tarjeta)
        {
            var CookCitaID = ControllerContext.HttpContext.Request.Cookies["IDCita"];
            string CitaID = CookCitaID.Value;
            CookCitaID.Expires = DateTime.Now.AddDays(-1D);

            var CookPsicologoID = ControllerContext.HttpContext.Request.Cookies["IDPsicologo"];
            string PsicologoID = CookPsicologoID.Value;
            CookPsicologoID.Expires = DateTime.Now.AddDays(-1D);

            Pago pago = new Pago
            {
                PagoMonto = tarjeta.MT,
                PagoFecha = DateTime.Now,
                Paciente = new Paciente{
                    PacienteID = SessionHelper.GetNameIdentifier(User)
                },
                Cita = new Cita{
                    CitaID = CitaID
                },
                Psicologo = new Psicologo{
                    PsicologoID = PsicologoID
                }
            };

            SecurionPayGateway gateway = new SecurionPayGateway("sk_test_MZm7slzOHYNJsvQ0yQjHaYwS");

            int montoPago = (int)(tarjeta.MT * 100);
            ChargeRequest request = new ChargeRequest()
            {
                Amount = montoPago,
                Currency = "PEN",
                Card = new CardRequest()
                {
                    CustomerId = SessionHelper.GetEmail(User),
                    Number = tarjeta.CN,
                    ExpMonth = tarjeta.NM.ToString(),
                    ExpYear = tarjeta.NY.ToString(),
                    Cvc = tarjeta.CV.ToString()
                }
            };

            try
            {
                Charge charge = await gateway.CreateCharge(request);

                // do something with charge object - see https://securionpay.com/docs/api#charge-object
                string chargeId = charge.Id;

                client = new FireSharp.FirebaseClient(config);
                //Crear codigo y cita
                PushResponse response = client.Push("Pago/", pago);
                pago.PagoID = response.Result.name;
                //Enviar datos de Cita a DB
                SetResponse setResponse = client.Set("Pago/" + pago.PagoID, pago);

                return RedirectToAction("SolicitarCita", "Cita", new { CitaID });
            }
            catch (SecurionPayException e)
            {
                // handle error response - see https://securionpay.com/docs/api#error-object
                ErrorType errorType = e.Error.Type;
                ErrorCode? errorCode = e.Error.Code;
                string errorMessage = e.Error.Message;
            }
            return RedirectToAction("CancelarPago","Cita");
        }

        [HttpGet]
        [Authorize(Roles = "Paciente")]
        public ViewResult AceptadoPago()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Paciente")]
        public ViewResult CancelarPago()
        {
            return View();
        }

        [Authorize]
        public ActionResult IngresarCita(string UID)
        {
            HttpCookie cookie = new HttpCookie("UsUID", UID);
            cookie.Domain = ".localhost:29346";
            cookie.Expires = DateTime.Now.AddHours(2);
            return Redirect("http://localhost:29346/WebRTC/Ingresar");
        }
    }
}