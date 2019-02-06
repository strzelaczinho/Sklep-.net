using SKLEP.Models.Data;
using SKLEP.Models.ViewModels.Strony;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SKLEP.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index","Pages");
        }
        public ActionResult KategoriePartial()
        {
            List<KategorieVM> kategorieVMs;

            using (Db db = new Db())
            {
                kategorieVMs = db.Kategorie.ToArray().OrderBy(x => x.Sortowanie).Select(x => new KategorieVM(x)).ToList();

            }

            //Return partial
            return PartialView(kategorieVMs);
        }
        //Shop/Kategorie/name
        public ActionResult Kategorie(string name)
        {
            // lista produktow 
            List<ProduktyVM> produktyVMs;

            using (Db db = new Db())
            {
                KategorieDTO dto = db.Kategorie.Where(x => x.Slug == name).FirstOrDefault();
                if (dto == null)
                    {
                    return RedirectToAction("Kategorie");
                }
                int kateId = dto.Id;

                //inicjaizacja
                produktyVMs = db.Produkty.ToArray().Where(x => x.KategoriaId == kateId).Select(x => new ProduktyVM(x)).ToList();

                var produktKat = db.Produkty.Where(x => x.KategoriaId == kateId).FirstOrDefault();
                ViewBag.CategoryName = produktKat.KategoriaNazwa;
            }

                //pobieram kategorie id
                return View(produktyVMs);
        }
        //get /shop/product-detail/name
        [ActionName("produkt-detail")] // metoda nazywa sie ProductDetails ale mapuja ja na product-details
        public ActionResult ProductDetails(string name)
        {
            //Deklaruje DTO aby wywolac konstruktor VM 

            ProduktyVM modelVM;
            ProduktyDTO dto;

            int id = 0;
            using (Db db =  new Db())
             {
                if (!db.Produkty.Any(x=> x.Slug.Equals(name))) // jesli zaden produkt nie pasuje
                {

                    return RedirectToAction("Index", "Shop");//powracam do metody Index kontrolera SHop
                }
                dto = db.Produkty.Where(x => x.Slug == name).FirstOrDefault();

                id = dto.Id;
                modelVM = new ProduktyVM(dto);
                modelVM.GaleriaZdjecia = Directory.EnumerateFiles(Server.MapPath("~/Zdjecia/Uploads/Produkty/" + id + "/Galeria/Thumbs")).Select(fn => Path.GetFileName(fn));

            }
            return View("ProductDetails",modelVM);
        }
    }
}