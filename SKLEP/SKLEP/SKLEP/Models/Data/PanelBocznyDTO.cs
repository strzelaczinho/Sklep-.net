using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SKLEP.Models.Data
{
    [Table("tabelkaPanelu")]
    public class PanelBocznyDTO
    {
        [Key]
        public int Id { get; set; }
        public string Opis { get; set; }
    }
}