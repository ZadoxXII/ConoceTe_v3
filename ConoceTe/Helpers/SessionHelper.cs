using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Security.Principal;

namespace ConoceTe.Helpers
{
    public class SessionHelper
    {

        public static string GetNameIdentifier(IPrincipal Usuario)
        {
            var x = ((ClaimsIdentity)Usuario.Identity).FindFirst(ClaimTypes.NameIdentifier);
            return x == null ? "" : x.Value;
        }

        public static string GetName(IPrincipal Usuario)
        {
            var x = ((ClaimsIdentity)Usuario.Identity).FindFirst(ClaimTypes.Name);
            return x == null ? "" : x.Value;
        }

        public static string GetEmail(IPrincipal Usuario)
        {
            var x = ((ClaimsIdentity)Usuario.Identity).FindFirst(ClaimTypes.Email);
            return x == null ? "" : x.Value;
        }

        public static string GetRol(IPrincipal Usuario)
        {
            var x = ((ClaimsIdentity)Usuario.Identity).FindFirst(ClaimTypes.Role);
            return x == null ? "" : x.Value;
        }
    }
}