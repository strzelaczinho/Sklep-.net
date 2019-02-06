using SKLEP.Models.Data;
using SKLEP.Models.ViewModels.Strony;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using SKLEP.Areas.Admin.Models.ViewModels;
using SKLEP.Models.ViewModels.Sklep;

namespace SKLEP.Areas.Admin.Controllers
{
      [Authorize(Roles="Admin")]
    public class SklepowyController : Controller
    {
        // GET: Admin/Sklepowy/Kategorie
        public ActionResult Kategorie()
        {
            List<KategorieVM> listaWidokow;

            using (Db db = new Db())
            {
                listaWidokow = db.Kategorie.ToArray().OrderBy(x => x.Sortowanie).Select(x => new KategorieVM(x)).ToList();

            }
            return View(listaWidokow);
        }

        //POST Admin/Sklepowu/DodajNowaKategorie
        [HttpPost]
        public string DodajNowaKategorie(string catname)
        {
            string id;
            using (Db db = new Db())
            {
                if (db.Kategorie.Any(x => x.Nazwa == catname))

                    return "titletaken";

                // Init DTO 
                KategorieDTO dto = new KategorieDTO();

                //Dodaje 
                dto.Nazwa = catname;
                dto.Slug = catname.Replace(" ", "-").ToLower();
                dto.Sortowanie = 100;

                //Sejw
                db.Kategorie.Add(dto);
                db.SaveChanges();

                id = dto.Id.ToString();
            }

            return id;
        }
        public ActionResult UsuwanieKategori(int id)
        {
            KategorieDTO dto;

            using (Db db = new Db())
            {
                dto = db.Kategorie.Find(id);
                db.Kategorie.Remove(dto);
                db.SaveChanges();
            }

            return RedirectToAction("Kategorie");
        }
        //POST Admin/Sklepowu/ReorderKategorie
        //
        [HttpPost]
        public void ReorderKategorie(int[] id)
        {
            using (Db db = new Db())
            {
                //Licznik poczatkowy
                int count = 1;

                //StronaDTO
                KategorieDTO dto;

                // Ustawiam sortowanie dla kazdej strony
                foreach (var pageId in id)
                {
                    dto = db.Kategorie.Find(pageId);
                    dto.Sortowanie = count;
                    db.SaveChanges();
                    count++;
                }
            }

        }
        // POST: Admin/Sklepowy/RenameCategory
        [HttpPost]
        public string ZmianaNazwyKategori(string newCatName, int id)
        {
            using (Db db = new Db())
            {
                // Check category name is unique
                if (db.Kategorie.Any(x => x.Nazwa == newCatName))
                    return "titletaken";

                // Get DTO
                KategorieDTO dto = db.Kategorie.Find(id);

                // Edit DTO
                dto.Nazwa = newCatName;
                dto.Slug = newCatName.Replace(" ", "-").ToLower();

                // Save
                db.SaveChanges();
            }

            // Return
            return "ok";
        }
        //GET Admin/Sklepowy/DodajProdukt
        [HttpGet]
        public ActionResult DodajProdukt()
        {
            ProduktyVM model = new ProduktyVM();
            using (Db db = new Db())
            {
                model.Kategorie = new SelectList(db.Kategorie.ToList(), "Id", "Nazwa");
            }

            return View(model);
        }
        //        Admin/Sklepowy/DodajProdukt
        [HttpPost]
        public ActionResult DodajProdukt(ProduktyVM model, HttpPostedFileBase file) // przekazuje zdjecie jako 2 argument 
        {
            //Sprawdzam stan modelu
            if (!ModelState.IsValid)
            {
                using (Db db = new Db())
                {
                    model.Kategorie = new SelectList(db.Kategorie.ToList(), "Id", "Nazwa");
                    return View(model); // jesli model nie przejdzie weryfikacji
                }

            }
            //Upewniam sie ze nazwa produktu jest unikalna
            using (Db db = new Db())
            {
                if (db.Produkty.Any(x => x.Nazwa == model.Nazwa))
                {
                    model.Kategorie = new SelectList(db.Kategorie.ToList(), "Id", "Nazwa");
                    ModelState.AddModelError("", "Ta nazwa produktu została juz wybrana");
                    return View(model); // jesli model nie przejdzie weryfikacji
                }

            }
            //Deklaruje id produktu
            int id;
            using (Db db = new Db())
            {
                ProduktyDTO produkt = new ProduktyDTO();
                produkt.Nazwa = model.Nazwa;
                produkt.Slug = model.Nazwa.Replace(" ", "-").ToLower();
                produkt.Opis = model.Opis;
                produkt.Cena = model.Cena;
                produkt.KategoriaId = model.KategoriaId;

                KategorieDTO catDTO = db.Kategorie.FirstOrDefault(x => x.Id == model.KategoriaId);
                produkt.KategoriaNazwa = catDTO.Nazwa;

                db.Produkty.Add(produkt);
                db.SaveChanges();

                // get the id 
                id = produkt.Id;
            }

            // Set TempData message
            TempData["SM"] = "Udalo ci się dodać produkt";

            //upload Image podczas dodawania produktu
            #region Upload Image
            //Tworze strukture katalogow
            var originalDirectory = new DirectoryInfo(string.Format("{0}Zdjecia\\Uploads", Server.MapPath(@"\")));

            //ustawiam foldery struktury katalogow
            var pathString1 = Path.Combine(originalDirectory.ToString(), "Produkty");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Produkty\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Produkty\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Produkty\\" + id.ToString() + "\\Galeria");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Produkty\\" + id.ToString() + "\\Galeria\\Thumbs");

            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);

            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);

            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);

            if (!Directory.Exists(pathString4))
                Directory.CreateDirectory(pathString4);

            if (!Directory.Exists(pathString5))
                Directory.CreateDirectory(pathString5);

            // Sprawdz czy zdjecie zostalo dodane
            if (file != null && file.ContentLength > 0)
            {
                //Pobieram rozszerzenie pliku
                string ext = file.ContentType.ToLower();

                // sprawdzam rozszerzenie jesli nie pasuje do tych formatow opisanych ponizej

                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (Db db = new Db())
                    {
                        model.Kategorie = new SelectList(db.Kategorie.ToList(), "Id", "Nazwa");
                        ModelState.AddModelError("", "Zdjecie nie zostalo dodane , zla nazwa produktu.");
                        return View(model);
                    }
                }

                //Inicjalizuje nazwe zdjecia
                string imageName = file.FileName;

                // Serjwuje zdjecie do DTO
                using (Db db = new Db())
                {
                    ProduktyDTO dto = db.Produkty.Find(id);
                    dto.NazwaZdjecia = imageName;
                    db.SaveChanges();
                }

                // Ustawiam orginal oraz thumb sciezki
                var path = string.Format("{0}\\{1}", pathString2, imageName);
                var path2 = string.Format("{0}\\{1}", pathString3, imageName);

                // Save orgyinalne zdjecie
                file.SaveAs(path);

                // Tworze i sejwuje thumb
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);// na sciezce
            }
            #endregion

            return RedirectToAction("DodajProdukt");
        }
        public ActionResult Produkty(int? numer, int? catId)
        {
            // Deklaruje liste of ProduktyVM
            List<ProduktyVM> listaProduktowViewModel;

            // Ustaw Page Number
            var numerStrony = numer ?? 1;

            using (Db db = new Db())
            {
                // Inicjalizuje liste
                listaProduktowViewModel = db.Produkty.ToArray()
                                  .Where(x => catId == null || catId == 0 || x.KategoriaId == catId)
                                  .Select(x => new ProduktyVM(x)).ToList();

                // Populacja kategori prez liste
                ViewBag.Categories = new SelectList(db.Kategorie.ToList(), "Id", "Nazwa");

                // Ustaw zaznaczona kategorie
                ViewBag.SelectedCat = catId.ToString();
            }

            // Ustawiam Stronicowanie
            var onePageOfProducts = listaProduktowViewModel.ToPagedList(numerStrony, 3);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            // Return view z lista
            return View(listaProduktowViewModel);
        }
        //Post Admin/Sklepowy/EdycjaProduktu/id
        [HttpGet]
        public ActionResult EdycjaProduktu(int id)
        {
            //Deklaruje produktVM
            ProduktyVM model;
            //Pobieram produkt
            using (Db db = new Db())
            {
                ProduktyDTO dto = db.Produkty.Find(id);
                if (dto == null)// Sprawdzam czy istnieje
                {
                    return Content("Taki produkt nie istnieje");
                }
                model = new ProduktyVM(dto);
                // Tworze select list
                model.Kategorie = new SelectList(db.Kategorie.ToList(), "Id", "Nazwa");
                //Pobieram wszystkie zdjecia z galerii
                model.GaleriaZdjecia = Directory.EnumerateFiles(Server.MapPath("~/Zdjecia/Uploads/Produkty/" + id + "/Galeria/Thumbs")).Select(fn => Path.GetFileName(fn));
            }
            //Zwracam view model
            return View(model);
        }
        //Post Admin/Sklepowy/EdycjaProduktu/id
        [HttpPost]
        public ActionResult EdycjaProduktu(ProduktyVM model, HttpPostedFileBase file)
        {
            //Get produkt ID
            int id = model.Id;
            //Populacja kategorii select list oraz zdjec galerii
            using (Db db = new Db())
            {
                //Upewnij sie ze nazwa produktu jest unikatowa
                model.Kategorie = new SelectList(db.Kategorie.ToList(), "Id", "Nazwa");
            }
            model.GaleriaZdjecia = Directory.EnumerateFiles(Server.MapPath("~/Zdjecia/Uploads/Produkty/" + id + "/Galeria/Thumbs")).Select(fn => Path.GetFileName(fn));


            //Check model errors
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //Sprawdzam czy nazwa produktu jest unikatowa
            using (Db db = new Db())
            {
                if (db.Produkty.Where(x => x.Id != id).Any(x => x.Nazwa == model.Nazwa))
                {
                    ModelState.AddModelError("", "Nazwa produktu zostala wybrana");
                    return View(model);
                }
            }
            // Update produkt
            using (Db db = new Db())
            {
                ProduktyDTO dto = db.Produkty.Find(id);
                dto.Nazwa = model.Nazwa;
                dto.Slug = model.Nazwa.Replace(" ", "-").ToLower();
                dto.Cena = model.Cena;
                dto.Opis = model.Opis;
                dto.KategoriaId = model.KategoriaId;
                dto.NazwaZdjecia = model.NazwaZdjecia;

                KategorieDTO kategoriaDTO = db.Kategorie.FirstOrDefault(x => x.Id == model.KategoriaId);
                kategoriaDTO.Nazwa = kategoriaDTO.Nazwa;
                db.SaveChanges();

            }
            TempData["SM"] = "Udało Ci się dodać produkt";
            #region Image Upload

            // Sprawdz plik do uploadu
            if (file != null && file.ContentLength > 0)
            {

                // Pobierz rozszerzenie
                string ext = file.ContentType.ToLower();

                // sprwadz rozszerzenie
                if (ext != "image/jpg" && ext != "image/jpeg" && ext != "image/pjpeg" &&
                    ext != "image/gif" && ext != "image/x-png" && ext != "image/png")
                {
                    using (Db db = new Db())
                    {
                        ModelState.AddModelError("", "Zdjecie nie zostalo dodane bledne rozszerzenie zdjecia.");
                        return View(model);
                    }
                }

                // Set uplpad directory paths
                var originalDirectory = new DirectoryInfo(string.Format("{0}Zdjecia\\Uploads", Server.MapPath(@"\")));

                var pathString1 = Path.Combine(originalDirectory.ToString(), "Produkty\\" + id.ToString());
                var pathString2 = Path.Combine(originalDirectory.ToString(), "Produkty\\" + id.ToString() + "\\Thumbs");

                //Usun pliki z kategorii
                DirectoryInfo di1 = new DirectoryInfo(pathString1);
                DirectoryInfo di2 = new DirectoryInfo(pathString2);
                foreach (FileInfo file2 in di1.GetFiles())
                    file2.Delete();

                foreach (FileInfo file3 in di2.GetFiles())
                    file3.Delete();

                // Zasejwuj nazwe zdjecia
                string imageName = file.FileName;

                using (Db db = new Db())
                {
                    ProduktyDTO dto = db.Produkty.Find(id);
                    dto.NazwaZdjecia = imageName;

                    db.SaveChanges();
                }

                // Zasejwuj orginal i miniaturki zdjecia
                var path = string.Format("{0}\\{1}", pathString1, imageName);
                var path2 = string.Format("{0}\\{1}", pathString2, imageName);

                file.SaveAs(path);

                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }

            #endregion
            return RedirectToAction("EdycjaProduktu");
        }
        //Get /Admin/Sklepowy/UsunProdukt
        public ActionResult UsunProdukt(int id)
        {
            using (Db db = new Db())
            {
                ProduktyDTO dto = db.Produkty.Find(id);
                db.Produkty.Remove(dto);
                db.SaveChanges();
            }
            var oryginalnaSciezka = new DirectoryInfo(string.Format("{0}Zdjecia\\Uploads", Server.MapPath(@"\")));

            string pathString = Path.Combine(oryginalnaSciezka.ToString(), "Produkty\\" + id.ToString());

            if (Directory.Exists(pathString))
                Directory.Delete(pathString, true);

            //Redirect
            return RedirectToAction("Produkty");
        }
        [HttpPost]
        public void SejwujZdjeciaZGalerii(int id)
        {
            // przechodze po plikach
            foreach (string nazwaPliku in Request.Files)
            {
                HttpPostedFileBase plik = Request.Files[nazwaPliku];

                //Sprawdz czy nie jest nulem
                if (plik != null && plik.ContentLength > 0)
                {
                    //inicjalizuje plik potem  sprwadzam nule  //ustawiam sciezki
                    var oryginalnaSciekza = new DirectoryInfo(string.Format("{0}Zdjecia\\Uploads\\", Server.MapPath(@"\")));

                    string pathString1 = Path.Combine(oryginalnaSciekza.ToString(), "Produkty\\" + id.ToString() + "\\Galeria");
                    string pathString2 = Path.Combine(oryginalnaSciekza.ToString(), "Produkty\\" + id.ToString() + "\\Galeria\\Thumbs");


                    var path = string.Format("{0}\\{1}", pathString1, plik.FileName);
                    var path2 = string.Format("{0}\\{1}", pathString2, plik.FileName);

                    //Sejwuj oryginal oraz minaturke

                    plik.SaveAs(path);
                    WebImage img = new WebImage(plik.InputStream);
                    img.Resize(200, 200);
                    img.Save(path2);
                }
            }
        }
        // POST: Admin/Sklepowy/UsunZdjecie
        [HttpPost]
        public void UsunZdjecie(int id, string imageName)
        {
            string fullPath1 = Request.MapPath("~/Zdjecia/Uploads/Produkty/" + id.ToString() + "/Galeria/" + imageName);
            string fullPath2 = Request.MapPath("~/Zdjecia/Uploads/Produkty/" + id.ToString() + "/Galeria/Thumbs/" + imageName);

            if (System.IO.File.Exists(fullPath1))
                System.IO.File.Delete(fullPath1);

            if (System.IO.File.Exists(fullPath2))
                System.IO.File.Delete(fullPath2);
        }


        public ActionResult Orders()
        {
            // Init list of OrdersForAdminVM
            List<ZamowieniaAdminaVM> ordersForAdmin = new List<ZamowieniaAdminaVM>();

            using (Db db = new Db())
            {
                // Init list of OrderVM
                List<ZamowienieVM> orders = db.Zamowienia.ToArray().Select(x => new ZamowienieVM(x)).ToList();

                // Loop through list of OrderVM
                foreach (var order in orders)
                {
                    // Init product dict
                    Dictionary<string, int> productsAndQty = new Dictionary<string, int>();

                    // Declare total
                    decimal total = 0m;

                    // Init list of OrderDetailsDTO
                    List<ZamowieniaSzczegolyDTO> orderDetailsList = db.ZamowieniaSzczegoly.Where(X => X.ZamowieniaId == order.IdZamowienia).ToList();

                    // Get username
                    UzytkownikDTO user = db.Uzytkownik.Where(x => x.Id == order.UzytkownikId).FirstOrDefault();
                    string username = user.Username;

                    // Loop through list of OrderDetailsDTO
                    foreach (var orderDetails in orderDetailsList)
                    {
                        // Get product
                        ProduktyDTO product = db.Produkty.Where(x => x.Id == orderDetails.ProduktId).FirstOrDefault();

                        // Get product price
                        decimal price =  product.Cena;





                        // Get product name
                        string productName = product.Nazwa;

                        // Add to product dict
                        productsAndQty.Add(productName, orderDetails.LiczbaProduktow);

                        // Get total
                        total += orderDetails.LiczbaProduktow * price;
                    }

                    // Add to ordersForAdminVM list
                    ordersForAdmin.Add(new ZamowieniaAdminaVM()
                    {
                        OrderNumber = order.IdZamowienia,
                        Username = username,
                        Total = total,
                        ProductsAndQty = productsAndQty,
                        CreatedAt = order.GodzinaUtworzenia
                    });
                }
            }

            // Return view with OrdersForAdminVM list
            return View(ordersForAdmin);
        }
    }
}
