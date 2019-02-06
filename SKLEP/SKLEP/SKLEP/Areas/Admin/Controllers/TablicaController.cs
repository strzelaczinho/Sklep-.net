using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SKLEP.Areas.Admin.Controllers
{
   [Authorize(Roles="Admin")]
    public class TablicaController : Controller
    {
        // GET: Admin/Tablica
        public ActionResult Index()
        {

            return View();
        }
    }
}