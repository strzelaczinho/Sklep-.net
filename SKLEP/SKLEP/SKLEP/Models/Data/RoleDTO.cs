using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SKLEP.Models.Data
{
        [Table("Role")]
    public class RoleDTO
    {
                [Key]
        public int Id { get; set; }
        public string Nazwa { get; set; }
    }
}