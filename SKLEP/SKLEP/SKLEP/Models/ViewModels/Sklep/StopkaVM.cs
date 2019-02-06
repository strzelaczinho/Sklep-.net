using SKLEP.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SKLEP.Models.ViewModels.Sklep
{
    public class StopkaVM
    {
        public StopkaVM()
        {

        }
        public StopkaVM(StopkaDTO wiersz)
        {
            Id = wiersz.Id;
            NazwaStopki = wiersz.NazwaStopki;
            OpisStopki = wiersz.OpisStopki;
        }
        public int Id { get; set; }
        public string NazwaStopki { get; set; }
        public string OpisStopki { get; set; }
    }
}