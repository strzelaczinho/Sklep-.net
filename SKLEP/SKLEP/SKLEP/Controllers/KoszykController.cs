using SKLEP.Models.Data;
using SKLEP.Models.ViewModels.Koszyk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace SKLEP.Controllers
{
    public class KoszykController : Controller
    {
        public ProduktyDTO ProduktyDTO { get; private set; }
        // GET: Koszyk
        public ActionResult Index()
        {
            // Init the cart list
            var cart = Session["koszyk"] as List<KoszykVM> ?? new List<KoszykVM>();

            // Check if cart is empty
            if (cart.Count == 0 || Session["Koszyk"] == null)
            {
                ViewBag.Message = "Twoj koszyk jest pusty.";
                return View();
            }

            // Calculate total and save to ViewBag

            decimal total = 0m;

            foreach (var item in cart)
            {
                total += item.SumaProduktow;
            }

            ViewBag.GrandTotal = total;

            // Return view with list
            return View(cart);
        }
        public ActionResult KoszykPartial()
        {
            // Init CartVM
            KoszykVM model = new KoszykVM();

            // Init quantity
            int qty = 0;

            // Init price
            decimal price = 0m;

            // Check for cart session
            if (Session["koszyk"] != null)
            {
                // Get total qty and price
                var list = (List<KoszykVM>)Session["koszyk"];

                foreach (var item in list)
                {
                    qty += item.Ilosc;
                    price += item.Ilosc * item.Cena;
                }

                model.Ilosc = qty;
                model.Cena = price;

            }
            else
            {
                // Or set qty and price to 0
                model.Ilosc = 0;
                model.Cena = 0m;
            }

            // Return partial view with model
            return PartialView(model);
        }
    
        public ActionResult DodajDoKoszykaPartial(int id)
        {
            // Init CartVM list
            List<KoszykVM> cart = Session["koszyk"] as List<KoszykVM> ?? new List<KoszykVM>();

            // Init CartVM
            KoszykVM model = new KoszykVM();

            using (Db db = new Db())
            {
                // Get the product
                ProduktyDTO product = db.Produkty.Find(id);

                // Check if the product is already in cart
                var productInCart = cart.FirstOrDefault(x => x.ProduktId == id);

                // If not, add new
                if (productInCart == null)
                {
                    cart.Add(new KoszykVM()
                    {
                        ProduktId = product.Id,
                        ProduktNazwa = product.Nazwa,
                        Ilosc = 1,
                        Cena = product.Cena,
                        Zdjecie = product.NazwaZdjecia
                    });
                }
                else
                {
                    // If it is, increment
                    productInCart.Ilosc++;
                }
            }

            // Get total qty and price and add to model

            int qty = 0;
            decimal price = 0m;

            foreach (var item in cart)
            {
                qty += item.Ilosc;
                price += item.Ilosc * item.Cena;
            }

            model.Ilosc = qty;
            model.Cena = price;

            // Save cart back to session
            Session["koszyk"] = cart;

            // Return partial view with model
            return PartialView(model);
        }

        public JsonResult ZwiekszProdukt(int productId)
        {
            List<KoszykVM> cart = Session["koszyk"] as List<KoszykVM>;

            using (Db db = new Db())
            {
                // Get cartVM from list
                KoszykVM model = cart.FirstOrDefault(x => x.ProduktId == productId);

                // Increment qty
                model.Ilosc++;

                // Store  data
                var result = new { qty = model.Ilosc, price = model.Cena };

                // Return json with data
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        // GET: /koszyk/ZmniejszProdukt
        public ActionResult ZmniejszProdukt(int productId)
        {
            // Init 
            List<KoszykVM> cart = Session["koszyk"] as List<KoszykVM>;

            using (Db db = new Db())
            {
                // Get model 
                KoszykVM model = cart.FirstOrDefault(x => x.ProduktId == productId);

                // Decrement qty
                if (model.Ilosc > 1)
                {
                    model.Ilosc--;
                }
                else  
                {
                    model.Ilosc = 0;
                    cart.Remove(model);
                }

                // Store  data
                var result = new { qty = model.Ilosc, price = model.Cena };

                // Return json
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        // GET: /koszyk/UsunProdukt
        public void UsunProdukt(int productId)
        {
            // Init 
            List<KoszykVM> cart = Session["koszyk"] as List<KoszykVM>;

            using (Db db = new Db())
            {
                // Get model 
                KoszykVM model = cart.FirstOrDefault(x => x.ProduktId == productId);

                // Remove model 
                cart.Remove(model);
            }

        }
        public ActionResult PayPalPartial()
        {
            List<KoszykVM> koszyk = Session["koszyk"] as List<KoszykVM>;

            return PartialView(koszyk);
        }
        //get /Koszyk/PlaceOrder
        [HttpPost]
        public void PlaceOrder()
        {
            // Get cart list
            List<KoszykVM> cart = Session["koszyk"] as List<KoszykVM>;

            // Get username
            string username = User.Identity.Name;

            int orderId = 0;

            using (Db db = new Db())
            {
                // Init OrderDTO
                ZamowieniaDTO orderDTO = new ZamowieniaDTO();

                // Get user id
                var q = db.Uzytkownik.FirstOrDefault(x => x.Username == username);
                int userId = q.Id;

                // Add to OrderDTO and save
                orderDTO.UzytkownikId = userId;
                orderDTO.GodzinaUtworzenia = DateTime.Now;

                db.Zamowienia.Add(orderDTO);

                db.SaveChanges();

                // Get inserted id
                orderId = orderDTO.IdZamowienia;

                // Init OrderDetailsDTO
                ZamowieniaSzczegolyDTO orderDetailsDTO = new ZamowieniaSzczegolyDTO();

                // Add to OrderDetailsDTO
                foreach (var item in cart)
                {
                    orderDetailsDTO.ZamowieniaId = orderId;
                    orderDetailsDTO.UzytkownikId = userId;
                    orderDetailsDTO.ProduktId = item.ProduktId;
                    orderDetailsDTO.LiczbaProduktow = item.Ilosc;

                    db.ZamowieniaSzczegoly.Add(orderDetailsDTO);

                    db.SaveChanges();
                }
            }
            //email admin

            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("5d1890a011c5a3", "ff560ce3603598"),
                EnableSsl = true
            };
            client.Send("admin@example.com", "admin@example.com", "New Order", "Masz nowe zamowienie. Nr zamowienia to "+ orderId);
            //reset sesje 
            Session["koszyk"] = null;
            
        }

    }
}