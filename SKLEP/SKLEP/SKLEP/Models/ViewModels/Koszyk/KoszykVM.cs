using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SKLEP.Models.ViewModels.Koszyk
{
    public class KoszykVM
    {
        public int ProduktId { get; set; }
        public string ProduktNazwa { get; set; }
        public int Ilosc { get; set; }
        public decimal Cena { get; set; }
        public decimal SumaProduktow { get { return Ilosc * Cena; } }
        public string Zdjecie { get; set; }
    }
}