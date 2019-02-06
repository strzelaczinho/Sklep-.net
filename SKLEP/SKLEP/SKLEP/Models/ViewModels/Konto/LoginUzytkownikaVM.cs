using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SKLEP.Models.ViewModels.Konto
{
    public class LoginUzytkownikaVM
    {
        [Required]
        public string NazwaUzytkownika { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RemeberMe { get; set; }
    }
}