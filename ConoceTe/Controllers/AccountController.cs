using ConoceTe.WebApp.Models;
using Firebase.Auth;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ConoceTe.Controllers
{
    public class AccountController : Controller
    {
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "SY69Fjg4reYO7Fvjm9Z9A9L0eJ0KxJXWUjxclvRu",
            BasePath = "https://conocete-a0251.firebaseio.com/"
        };
        IFirebaseClient client;

        private static string ApiKey = "AIzaSyB9z_O4gKDgp_EX9WNL4MiOCkrNfRiKrdE";
        //private static string Bucket = "conocete-a0251.firebaseio.com";

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string email, string contra)
        {
            try
            {
                // Verification.
                if (ModelState.IsValid)
                {
                    var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(ApiKey));
                    var ab = await auth.SignInWithEmailAndPasswordAsync(email, contra);
                    string token = ab.FirebaseToken;
                    var user = ab.User;
                    if (token != null)
                    {
                        this.SignInUser(user.Email, token, user.LocalId, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "La contraseña o el usuario es incorrecto.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }
            ViewBag.ErrorLogin = "El correo electrónico o la contraseña son invalidos <i class='fa fa-grav' aria-hidden='true'></i>";
            return View();
        }
        private void SignInUser(string email, string token, string uid, bool isPersistent)
        {
            // Initialization.
            var claims = new List<Claim>();

            try
            {
                client = new FireSharp.FirebaseClient(config);
                var UsuarioJSON = client.Get("Usuario/" + uid);
                Usuario usuario = UsuarioJSON.ResultAs<Usuario>();

                // Setting
                claims.Add(new Claim(ClaimTypes.Email, email));
                claims.Add(new Claim(ClaimTypes.Name, usuario.UsuarioNombre + " " +usuario.UsuarioApellido));
                claims.Add(new Claim(ClaimTypes.Authentication, token));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, uid));
                switch (usuario.UsuarioRol)
                {
                    case "Administrador":
                        claims.Add(new Claim(ClaimTypes.Role, "Administrador"));
                        break;
                    case "Psicologo":
                        claims.Add(new Claim(ClaimTypes.Role, "Psicologo"));
                        break;
                    default:
                        claims.Add(new Claim(ClaimTypes.Role, "Paciente"));
                        break;
                }
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;

                // Registrar.
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }
        }

        private void ClaimIdentities(string username, bool isPersistent)
        {
            // Initialization.
            var claims = new List<Claim>();
            try
            {
                // Setting
                claims.Add(new Claim(ClaimTypes.Name, username));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RegisterPsicologo()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterPsicologo(Usuario usuario)
        {
            string NomYAPe = usuario.UsuarioNombre + " " + usuario.UsuarioApellido;
            usuario.UsuarioRol = "Psicologo";
            usuario.UsuarioEstado = "Ps00"; //Estado inaccesible. Necesita Ps01
            try
            {
                var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(ApiKey));
                //Crea Usuario Para autentificar al ingresar
                var a = await auth.CreateUserWithEmailAndPasswordAsync(usuario.UsuarioEmail, usuario.UsuarioContra, NomYAPe, true);
                //Extraer UID para ID en base de datos
                usuario.UsuarioID = a.User.LocalId;
                ModelState.AddModelError(string.Empty, "Excelente. Por favor, verifique su correo eletrónico.");

                //Crea USUARIO Psicologo en la base de datos
                AddPsicologoToFirebase(usuario);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex); //"Lamento decirle que algo ha ocurrido. Por favor, vuelva a intentarlo."
            }
            return View();
        }
        private void AddPsicologoToFirebase(Usuario usuario)
        {
            client = new FireSharp.FirebaseClient(config);
            var DatosPsicologo = usuario;
            DatosPsicologo.Psicologo.PsicologoID = DatosPsicologo.UsuarioID;
            SetResponse setResponse = client.Set("Usuario/" + DatosPsicologo.UsuarioID, DatosPsicologo);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RegisterPaciente()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterPaciente(Usuario usuario)
        {
            string NomYAPe = usuario.UsuarioNombre + " " + usuario.UsuarioApellido;
            usuario.UsuarioRol = "Paciente";
            usuario.UsuarioEstado = "Pa01";
            try
            {
                var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(ApiKey));
                //Crea Usuario Para autentificar al ingresar
                var a = await auth.CreateUserWithEmailAndPasswordAsync(usuario.UsuarioEmail, usuario.UsuarioContra, NomYAPe, true);
                //Extraer UID para ID en base de datos
                usuario.UsuarioID = a.User.LocalId;
                ModelState.AddModelError(string.Empty, "Excelente. Por favor, verifique su correo eletrónico.");

                //Crea USUARIO Paciente en la base de datos
                AddPacienteToFirebase(usuario);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }
        private void AddPacienteToFirebase(Usuario usuario)
        {
            client = new FireSharp.FirebaseClient(config);
            var DatosPaciente = usuario;
            DatosPaciente.Paciente.PacienteID = DatosPaciente.UsuarioID;
            SetResponse setResponse = client.Set("Usuario/" + DatosPaciente.UsuarioID, DatosPaciente);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string email)
        {

            return View();
        }

        [HttpGet]
        //[Authorize]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }
    }
}