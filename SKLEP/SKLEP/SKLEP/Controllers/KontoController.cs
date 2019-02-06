using SKLEP.Models.Data;
using SKLEP.Models.ViewModels.Konto;
using SKLEP.Models.ViewModels.Sklep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SKLEP.Controllers
{
    
    public class KontoController : Controller
    {
        // GET: Konto
        public ActionResult Index()
        {
            return RedirectToAction("~/Konto/login");
        }
        // get Konto/Login
        [HttpGet]
        public ActionResult Login()
        {
            //Powierdzam czy uzytkownik jest zalogowany

            string username = User.Identity.Name;// metoda z framerworka stwierdzajaca czy uzytkownik sie zalogowal
            if (!string.IsNullOrEmpty(username))
            {
                return RedirectToAction("user-profile");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginUzytkownikaVM model)
        {
            //Sprawdz stan modelu 
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            bool isValid = false;
            using (Db db = new Db())
            {
                if (db.Uzytkownik.Any(x=> x.Username.Equals(model.NazwaUzytkownika) && x.Password.Equals(model.Password)))
                    {
                    isValid = true;
                    }
            }
            if (!isValid)
            {
                ModelState.AddModelError("", "Nieprawidlowy uzytkownik lub haslo");
                return View(model);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(model.NazwaUzytkownika, model.RemeberMe); //sesja :) . Ustawiam sesje
                return Redirect(FormsAuthentication.GetRedirectUrl(model.NazwaUzytkownika,model.RemeberMe));
            }
        }
        [Authorize]//dla wszystkich rol sie tyczy
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/konto/login");
        }

        //get Konto/stworz-konto
        [ActionName("stworz-konto")]
        [HttpGet]
        public ActionResult StworzKonto()
        {


            return View("StworzKonto");
        }

        //post Konto/stworz-konto
        [ActionName("stworz-konto")]
        [HttpPost]
        public ActionResult StworzKonto(UzytkownikVM model)
        {
            //Sprawdzam stan modelu
            if(!ModelState.IsValid)
            {
                return View("StworzKonto", model);
            }
            //Sprawdzam haslo
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("","Hasła się nie zgadzają");
                return View("StworzKonto", model);
            }
            //Sprawdzam czy user to nawzwa unikana
            using (Db db = new Db())
            {
               if (db.Uzytkownik.Any(x=> x.Username.Equals(model.Username)))
                {
                    // to jest problem bo pasuja do siebie nazwy w bazie danych wiec drukuje
                    ModelState.AddModelError("", "Taki user "+model.Username+" został juz wybrany");
                    model.Username = "";
                    return View("StworzKonto", model);
                }
                // Create UserDTO
                UzytkownikDTO userDTO = new UzytkownikDTO()
                {
                    Imie = model.Username,
                    Nazwisko = model.Nazwisko,
                    EmailAddress = model.EmailAddress,
                    Username = model.Username,
                    Password = model.Password
                };
                //Dodaje
                db.Uzytkownik.Add(userDTO);
                db.SaveChanges();     //Save
                //Dodaje do rol
                int id = userDTO.Id;
                RoleKluczeGlowneDTO uzytkownik = new RoleKluczeGlowneDTO()
                {
                    UzytkownikID = id,
                    RolaID = 2
                };
                db.RolaKlucze.Add(uzytkownik);
                db.SaveChanges();
                //Tworze temp message
            }
            TempData["SM"] = "Jestes teraz zarejestrowany i możesz się zalgowować";
            //Redirect
            return Redirect("~/konto/login");
        }
        [Authorize]
        public ActionResult UserNavPaRtial()
        {
            //get username 
            string username = User.Identity.Name;
            UserNavPartialVM model;
            //deklaruje model
            using (Db db = new Db())
            {
                //budowa modelu 
                UzytkownikDTO dto = db.Uzytkownik.FirstOrDefault(x => x.Username == username);

                model = new UserNavPartialVM()
                {
                    Imie = dto.Imie,
                    Nazwisko = dto.Nazwisko
                };
            }
            //Return partial view z modelem
            return PartialView(model);
        }

        //get konto/user-profile
        [HttpGet]
        [ActionName("user-profile")]
        [Authorize]
        public ActionResult UserProfile()
        {

            //get username
            string username = User.Identity.Name;
            //Deklaruj models
            ProfilUyztkownikaVM model;
            using (Db db = new Db())
            {
                // get user 

                UzytkownikDTO dto = db.Uzytkownik.FirstOrDefault(x => x.Username == username);

                //build model
                model = new ProfilUyztkownikaVM(dto);
            }

                //Build

                // Return view z modelem
                return View("UserProfile",model);
        }
        // POST: /account/user-profile
        [HttpPost]
        [ActionName("user-profile")]
        [Authorize]
        public ActionResult UserProfile(ProfilUyztkownikaVM model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View("UserProfile", model);
            }

            // Check if passwords match if need be
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Hasła się nie zgadzają");
                    return View("UserProfile", model);
                }
            }

            using (Db db = new Db())
            {
                // Get username
                string username = User.Identity.Name;

                // Make sure username is unique
                if (db.Uzytkownik.Where(x => x.Id != model.Id).Any(x => x.Username == username))
                {
                    ModelState.AddModelError("", "Username " + model.Username + " juz istnieje.");
                    model.Username = "";
                    return View("UserProfile", model);
                }

                // Edit DTO
                UzytkownikDTO dto = db.Uzytkownik.Find(model.Id);

                dto.Imie = model.Imie;
                dto.Nazwisko = model.Nazwisko;
                dto.EmailAddress = model.EmailAddress;
                dto.Username = model.Username;

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    dto.Password = model.Password;
                }

                // Save
                db.SaveChanges();
            }

            // Set TempData message
            TempData["SM"] = "Edytowales wlasnie swoj profil!";

            // Redirect
            return Redirect("~/konto/user-profile");
        }

        // GET: /account/Orders
        [Authorize(Roles = "Uzytkownik")]
        public ActionResult Orders()
        {
            // Init list of OrdersForUserVM
            List<ZamowieniaDlaUzytkownikaVM> ordersForUser = new List<ZamowieniaDlaUzytkownikaVM>();

            using (Db db = new Db())
            {
                // Get user id
                UzytkownikDTO user = db.Uzytkownik.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
                int userId = user.Id;

                // Init list of OrderVM
                List<ZamowienieVM> orders = db.Zamowienia.Where(x => x.UzytkownikId == userId).ToArray().Select(x => new ZamowienieVM(x)).ToList();

                // Loop through list of OrderVM
                foreach (var order in orders)
                {
                    // Init products dict
                    Dictionary<string, int> productsAndQty = new Dictionary<string, int>();

                    // Declare total
                    decimal total = 0m;

                    // Init list of OrderDetailsDTO
                    List<ZamowieniaSzczegolyDTO> orderDetailsDTO = db.ZamowieniaSzczegoly.Where(x => x.ZamowieniaId == order.IdZamowienia).ToList();

                    // Loop though list of OrderDetailsDTO
                    foreach (var orderDetails in orderDetailsDTO)
                    {
                        // Get product
                        ProduktyDTO product = db.Produkty.Where(x => x.Id == orderDetails.ProduktId).FirstOrDefault();

                        // Get product price
                        decimal price = product.Cena;

                        // Get product name
                        string productName = product.Nazwa;

                        // Add to products dict
                        productsAndQty.Add(productName, orderDetails.LiczbaProduktow);

                        // Get total
                        total += orderDetails.LiczbaProduktow * price;
                    }

                    // Add to OrdersForUserVM list
                    ordersForUser.Add(new ZamowieniaDlaUzytkownikaVM()
                    {
                        OrderNumber = order.IdZamowienia,
                        Total = total,
                        ProductsAndQty = productsAndQty,
                        CreatedAt = order.GodzinaUtworzenia
                    });
                }

            }

            // Return view with list of OrdersForUserVM
            return View(ordersForUser);
        }
    }
}