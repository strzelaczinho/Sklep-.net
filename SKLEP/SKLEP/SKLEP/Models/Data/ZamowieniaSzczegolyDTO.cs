using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SKLEP.Models.Data
{
    [Table("ZamowieniaSzczegoly")]
    public class ZamowieniaSzczegolyDTO
    {
        [Key]
        public int Id { get; set; }
        public int ZamowieniaId { get; set; }
        public int UzytkownikId{get;set;}
        public int ProduktId { get; set; }
        public int LiczbaProduktow { get; set; }


        [ForeignKey("ZamowieniaId")]
        public virtual UzytkownikDTO Zamowienia { get; set; }
        [ForeignKey("ProduktId")]
        public virtual UzytkownikDTO Produkty { get; set; }
        [ForeignKey("UzytkownikId")]
        public virtual UzytkownikDTO Uzytkownicy { get; set; }
    }
}