using SKLEP.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SKLEP.Models.ViewModels.Strony
{
    public class ProduktyVM
    {

        public ProduktyVM()
        {

        }
        public ProduktyVM(ProduktyDTO wiersz)
        {
            Id = wiersz.Id;
            Nazwa = wiersz.Nazwa;
            Slug = wiersz.Slug;
            Opis = wiersz.Opis;
            Cena = wiersz.Cena;
            KategoriaNazwa = wiersz.KategoriaNazwa;
            KategoriaId = wiersz.KategoriaId;
            NazwaZdjecia = wiersz.NazwaZdjecia;
        }
        
        public int Id { get; set; }
        [Required]
        public string Nazwa { get; set; }
        public string Slug { get; set; }
        [Required]
        public string Opis { get; set; }
        public decimal Cena { get; set; }
        public string KategoriaNazwa { get; set; }
        [Required]
        public int KategoriaId { get; set; }
        public string NazwaZdjecia { get; set; }

        public IEnumerable<SelectListItem> Kategorie { get; set; }
        public IEnumerable<String> GaleriaZdjecia { get; set; }
    }
}