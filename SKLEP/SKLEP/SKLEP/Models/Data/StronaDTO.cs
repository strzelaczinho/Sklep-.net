using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SKLEP.Models.Data
{
    [Table("tabelkaStron")]
    public class StronaDTO
    {
            [Key]
            public int Id { get; set; }
            public string Tytuł { get; set; }
            public string Slug { get; set; }
            public string Opis { get; set; }
            public int Sortowanie { get; set; }
            public bool CzyPosiadaPanel { get; set; }
    }
}