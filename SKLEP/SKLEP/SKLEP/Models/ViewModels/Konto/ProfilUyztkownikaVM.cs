using SKLEP.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SKLEP.Models.ViewModels.Konto
{
    public class ProfilUyztkownikaVM
    {
        public ProfilUyztkownikaVM()
        {

        }
        public ProfilUyztkownikaVM(UzytkownikDTO wiersz)
        {
            Id = wiersz.Id;
            Imie = wiersz.Imie;
            Nazwisko = wiersz.Nazwisko;
            EmailAddress = wiersz.EmailAddress;
            Username = wiersz.Username;
            Password = wiersz.Password;
        }

        public int Id { get; set; }
        [Required]
        public string Imie { get; set; }
        [Required]
        public string Nazwisko { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}