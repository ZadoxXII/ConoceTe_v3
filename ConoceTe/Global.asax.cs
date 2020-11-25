using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ConoceTe
{
    public class MvcApplication : System.Web.HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Application["Aplicaciones"] = 0;
            //Cuantas sesiones estan siendo usado
            Application["SesionesUsuario"] = 0;

            //Cada Inicio de sesion +1
            Application["Aplicaciones"] = (int)Application["Aplicaciones"] + 1;
        }

        //Al iniciar sesion
        void Session_Star (object sender, EventArgs e)
        {
            //Cada Inicio de sesion +1 * Usuario
            Application["SesionesUsuario"] = (int)Application["SesionesUsuario"] + 1;
        }
        //Al Cerrar sesion
        void Session_End(object sender, EventArgs e)
        {
            //Cada Fin de sesion -1 * Usuario
            Application["SesionesUsuario"] = (int)Application["SesionesUsuario"] - 1;
        }
    }
}
