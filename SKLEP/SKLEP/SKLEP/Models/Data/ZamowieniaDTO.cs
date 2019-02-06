using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SKLEP.Models.Data
{
    [Table("Zamowienia")]
    public class ZamowieniaDTO
    {
         [Key]
        public int IdZamowienia { get; set; }
        public int UzytkownikId { get; set; }
        public DateTime GodzinaUtworzenia { get; set; }

        [ForeignKey("UzytkownikId")]
        public virtual UzytkownikDTO Users { get; set; }
    }
}