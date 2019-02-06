using SKLEP.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SKLEP.Models.ViewModels.Strony
{
    public class KategorieVM
    {
        public KategorieVM()
        {

        }
        public KategorieVM(KategorieDTO wiersz)
        {
            Id = wiersz.Id;
            Nazwa = wiersz.Nazwa;
            Slug = wiersz.Slug;
            Sortowanie = wiersz.Sortowanie;
        }
        [Key]
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public string Slug { get; set; }
        public int Sortowanie { get; set; }
    }
}