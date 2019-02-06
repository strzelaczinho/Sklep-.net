using SKLEP.Models.Data;
using SKLEP.Models.ViewModels.Strony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SKLEP.Controllers
{
    public class PagesController : Controller
    {
        public ActionResult Index(string page = "")
        {

            //get//set page slug
            if (page == "")
            {
                page = "home";
            }
            //model i dto
            StronaVM model;
            StronaDTO dto;
            //sprawdzam nula
            using (Db db = new Db())
            {
                if (!db.Strony.Any(x => x.Slug.Equals(page)))
                {
                    return RedirectToAction("Index", new { page = "" });
                }
            }
            //ustawiam tytul strony bo tutaj juz istnieje
            using (Db db = new Db())
            {
                dto = db.Strony.Where(x => x.Slug == page).FirstOrDefault();
            }
            ViewBag.PageTitle = dto.Tytuł;

            if (dto.CzyPosiadaPanel)
            {
                ViewBag.SideBar = "Yes";
            }
            else
            {
                ViewBag.SideBar = "No";
            }
            //sprawdzaam sidebar
            model = new StronaVM(dto);
            //zwracam model
            return View(model);
        }
        public ActionResult StronyPartial()
        {
            // Declare a list of PageVM
            List<StronaVM> pageVMList;

            // Get all pages except home
            using (Db db = new Db())
            {
                pageVMList = db.Strony.ToArray().OrderBy(x => x.Sortowanie).Where(x => x.Slug != "home").Select(x => new StronaVM(x)).ToList();
            }
            // Return partial view with list
            return PartialView(pageVMList);
        }
        public ActionResult PanelPartial()
        {
            PanelBocznyVM model;
            using (Db db = new Db())
            {
                PanelBocznyDTO dto = db.Boczny.Find(1);

                model = new PanelBocznyVM(dto);
            }

            return PartialView(model);
        }
    }
}