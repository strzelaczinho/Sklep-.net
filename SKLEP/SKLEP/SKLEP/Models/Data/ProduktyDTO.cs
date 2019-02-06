using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SKLEP.Models.Data
{
    [Table("Produkty")]
    public class ProduktyDTO
    {
        [Key]
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public string Slug { get; set; }
        public string Opis { get; set; }
        public decimal Cena { get; set; }
        public string KategoriaNazwa { get; set; }
        public int KategoriaId { get; set; }
        public string NazwaZdjecia { get; set; }

        [ForeignKey("KategoriaId")]
        public virtual KategorieDTO Kategoria { get; set; }
    }
}