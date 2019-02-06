using SKLEP.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SKLEP.Models.ViewModels.Strony
{
    public class PanelBocznyVM
    {
        public PanelBocznyVM()
        {

        }
        public PanelBocznyVM(PanelBocznyDTO wiersz)
        {
            Id = wiersz.Id;
            Opis = wiersz.Opis;
        }
        public int Id { get; set; }
        [AllowHtml]    // pozwala na edytowanie Pola w PaneluBocznym jako  CKEDITOR. Pozwala na uzywaine html w ckeditorze
        public string Opis { get; set; }
    }
}