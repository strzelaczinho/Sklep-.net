using SKLEP.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SKLEP.Models.ViewModels.Strony
{
    public class StronaVM
    {
        public StronaVM()
        {

        }
        public StronaVM(StronaDTO wiersz)
        {

            Id = wiersz.Id;
            Tytuł = wiersz.Tytuł;
            Slug = wiersz.Slug;
            Opis = wiersz.Opis;
            Sortowanie = wiersz.Sortowanie;
            CzyPosiadaPanel = wiersz.CzyPosiadaPanel;
        }
        public int Id { get; set; }
        [Required]
        [StringLength(50,MinimumLength =3)] // max 50 min 3
        public string Tytuł { get; set; }
        public string Slug { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)] // max 50 min 3
        [AllowHtml]
        public string Opis { get; set; }
        public int Sortowanie { get; set; }
        public bool CzyPosiadaPanel { get; set; }

        
    }
}