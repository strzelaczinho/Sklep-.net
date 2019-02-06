using SKLEP.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SKLEP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //metoda do uruchomienia , pozwolenia uzywania rol w sytemie
        protected void Application_AuthenticateRequest()
        {
            // Sprawdzam czy uzytkownik jest zalogowany 
            if (User == null) { return; }
            //pobieram usrname 

            string nazwaUzytkownika = Context.User.Identity.Name;
            //deklaruje liste rol
            string[] roles = null;
            //tworze populacje
            using (Db db = new Db())
            {
                UzytkownikDTO dto = db.Uzytkownik.FirstOrDefault(x => x.Username == nazwaUzytkownika);

                roles = db.RolaKlucze.Where(x => x.UzytkownikID == dto.Id).Select(x => x.Role.Nazwa).ToArray();
            }
            //build principal object
            IIdentity identityUzytkownika = new GenericIdentity(nazwaUzytkownika);
            IPrincipal newUzytkownikObj = new GenericPrincipal(identityUzytkownika, roles);

            Context.User = newUzytkownikObj;
         //   Console.WriteLine(newUzytkownikObj);
        }
    }
}
