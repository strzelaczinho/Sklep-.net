﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SKLEP.Models.Data
{
       [Table("tabelkaUzytkownika")]
    public class UzytkownikDTO
    {
        [Key]
        public int Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


    }
}