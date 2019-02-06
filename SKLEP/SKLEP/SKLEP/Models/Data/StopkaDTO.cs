using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SKLEP.Models.Data
{
    [Table("Stopka")]
    public class StopkaDTO
    {
        [Key]
        public int Id { get; set; }
        public string NazwaStopki { get; set; }
        public string OpisStopki { get; set; }
    }
}