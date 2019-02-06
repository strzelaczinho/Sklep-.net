using SKLEP.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SKLEP.Models.ViewModels.Sklep
{
    public class ZamowienieVM
    {
        public ZamowienieVM()
        {

        }
        public ZamowienieVM(ZamowieniaDTO wiersz)
        {
            IdZamowienia = wiersz.IdZamowienia;
            UzytkownikId = wiersz.UzytkownikId;
            GodzinaUtworzenia = wiersz.GodzinaUtworzenia;
        }
        public int IdZamowienia { get; set; }
        public int UzytkownikId { get; set; }
        public DateTime GodzinaUtworzenia { get; set; }
    }
}