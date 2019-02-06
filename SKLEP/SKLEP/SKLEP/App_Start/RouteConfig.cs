using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SKLEP
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
                                                                                            ///indeks jako domyslna akcja
            routes.MapRoute("Konto", "Konto/{action}/{id}", new { controller = "Konto", action = "Index", id = UrlParameter.Optional }, new[] { "SKLEP.Controllers" });
            routes.MapRoute("Koszyk", "Koszyk/{action}/{id}", new { controller = "Koszyk", action = "Index", id = UrlParameter.Optional }, new[] { "SKLEP.Controllers" });
            routes.MapRoute("Shop", "Shop/{action}/{name}", new { controller = "Shop", action = "Index", name = UrlParameter.Optional }, new[] { "SKLEP.Controllers" });//akcja i nazwa kategorii w mapowaniu
            routes.MapRoute("PanelPartial", "Pages/PanelPartial", new { controller = "Pages", action = "PanelPartial" }, new[] { "SKLEP.Controllers" });
            routes.MapRoute("StronyPartial", "Pages/StronyPartial", new { controller = "Pages", action = "StronyPartial" }, new[] { "SKLEP.Controllers" });
            routes.MapRoute("Pages", "{page}", new { controller = "Pages", action = "Index" }, new[] { "SKLEP.Controllers" });//aktualne zdjecia
            routes.MapRoute("Default", "", new { controller = "Pages", action = "Index" }, new[] { "SKLEP.Controllers" });//home

            /*     routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                ); */
        }
    }
}
