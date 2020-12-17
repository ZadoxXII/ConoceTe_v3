using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VideoChat.Controllers
{
    public class WebRTCController : Controller
    {
        [HttpGet]
        public ActionResult Index(string UID)
        {
            if (UID == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Ingresar()
        {
            return RedirectToAction("Index");
        }
       
        public ActionResult Error()
        {
            return View();
        }
    }
}
