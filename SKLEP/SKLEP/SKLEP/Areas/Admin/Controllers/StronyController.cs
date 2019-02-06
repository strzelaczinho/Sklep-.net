using SKLEP.Models.Data;
using SKLEP.Models.ViewModels.Strony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SKLEP.Areas.Admin.Controllers
{
  [Authorize(Roles = "Admin")]
    public class StronyController : Controller
    {
        // GET: Admin/Strony
        public ActionResult Index()
        {
            //Deklaruje liste stron StronaVM
            List<StronaVM> listaStron;

            //inicjalizacja 
            using (Db db = new Db())
            {
           listaStron = db.Strony.ToArray().OrderBy(x => x.Sortowanie).Select(x => new StronaVM(x)).ToList();
            }

            return View(listaStron); //zwracam ListeStron 
        }

        // GET: Admin/Strony/Dodawanie
        [HttpGet]
        public ActionResult Dodawanie()
        {
            return View();
        }
        // GET: Admin/Strony/Dodawanie
        [HttpPost]
        public ActionResult Dodawanie(StronaVM modelStrony)
        {
            if (!ModelState.IsValid) // jesli model nie jest ok , brakuej jakiegos pola etc :)
            {
                return View(modelStrony); ///zwracam jego stan z wypelnionym formularzem
            }

            using (Db db = new Db())
            {
                // Declare slug
                string slug;

                // Init pageDTO
                StronaDTO dto = new StronaDTO();

                // DTO title
                dto.Tytuł = modelStrony.Tytuł;

                // Check for and set slug if need be
                if (string.IsNullOrWhiteSpace(modelStrony.Slug))
                {
                    slug = modelStrony.Tytuł.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = modelStrony.Slug.Replace(" ", "-").ToLower();
                }

                // Make sure title and slug are unique
                if (db.Strony.Any(x => x.Tytuł == modelStrony.Tytuł) || db.Strony.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Taki tytuł lub slug juz istnieje");
                    return View(modelStrony);
                }

                // DTO the rest
                dto.Slug = slug;
                dto.Opis = modelStrony.Opis;
                dto.CzyPosiadaPanel = modelStrony.CzyPosiadaPanel;
                dto.Sortowanie = 100;

                // Save DTO
                db.Strony.Add(dto);
                db.SaveChanges();
            }
            TempData["SM"] = "Udalo ci się dodać stronę ";
            return RedirectToAction("Dodawanie");
        }
        // GET: Admin/Pages/Edycja/id
        [HttpGet]
        public ActionResult Edycja(int id)
        {
            // Declare pageVM
            StronaVM model;

            using (Db db = new Db())
            {
                //Pobieram strone
                StronaDTO dto = db.Strony.Find(id);

               // Potwierdzam ze istnieje, jesli nie to 
                if (dto == null)
                {
                    return Content("Strona nie istnieje");
                }

                // Init VM po konstruktorze
                model = new StronaVM(dto);
            }
            //zwracam widok z modelem
            return View(model);
        }

        // POST: Admin/Pages/Edycja/id
        [HttpPost]
        public ActionResult Edycja(StronaVM model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                int id = model.Id;
                // Init slug
                string slug = "home";
                //pobierz strone
                StronaDTO dto = db.Strony.Find(id);

                // pobieram tytul  z przeslanego modelu
                dto.Tytuł = model.Tytuł;

                // ustawiam sluga 
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Tytuł.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }
                //Upewniam sie ze tytul i slug to nazwy unikalne
                if (db.Strony.Where(x => x.Id != id).Any(x => x.Tytuł == model.Tytuł) ||
                     db.Strony.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Ten tytul lub slug juz istnieje.");
                    return View(model);
                }

                // DTO  rest
                dto.Slug = slug;
                dto.Opis = model.Opis;
                dto.CzyPosiadaPanel = model.CzyPosiadaPanel;

                // Save the DTO
                db.SaveChanges();
            }

            // Set TempData 
            TempData["SM"] = "Gratulacje udało  ci się edytować stronę";

            // Redirect
            return RedirectToAction("Edycja");
        }
        //Admin/Strony/Szczegóły/id
        public ActionResult Szczegóły(int id)
        {
            StronaVM model;

            using (Db db = new Db())
            {
                // pobieram strone
                StronaDTO dto = db.Strony.Find(id);

                // Sprawdzam czy istnieje jesli nie to 
                if (dto == null)
                {
                    return Content("Strona nie istnieje.");
                }

                // Init VM
                model = new StronaVM(dto);
            }
            // Return view with model
            return View(model);
        }
        //Admin/Strony/Usuwanie/id
        public ActionResult Usuwanie(int id)
        {
            using (Db db = new Db())
            {
                // pobieram strone
                StronaDTO dto = db.Strony.Find(id);

                // Usuwam ją
                db.Strony.Remove(dto);

                // Save
                db.SaveChanges();
            }

            // Redirect
            return RedirectToAction("Index");
        }

        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                //Licznik poczatkowy
                int count = 1;

                // StronaDTO
                StronaDTO dto;

                // Ustawiam sortowanie dla kazdej strony
                foreach (var pageId in id)
                {
                    dto = db.Strony.Find(pageId);
                    dto.Sortowanie = count;

                    db.SaveChanges();

                    count++;
                }
            }

        }
        //Admin/Strony/EdycjaPaneluBocznego
        [HttpGet]
        public ActionResult EdycjaPaneluBocznego()
        {
            PanelBocznyVM model;

            using (Db db = new Db())
            {
                PanelBocznyDTO dto = db.Boczny.Find(1);

                model = new PanelBocznyVM(dto);
            }

                return View(model);
        }
        [HttpPost]
        public ActionResult EdycjaPaneluBocznego(PanelBocznyVM model)
        {
            using (Db db = new Db())
            {
                PanelBocznyDTO panel = db.Boczny.Find(1);

                panel.Opis = model.Opis;
                db.SaveChanges();
            }
            TempData["SM"] = "Udało ci się edytować Panel Boczny";

            return RedirectToAction("EdycjaPaneluBocznego");

        }

    }
}