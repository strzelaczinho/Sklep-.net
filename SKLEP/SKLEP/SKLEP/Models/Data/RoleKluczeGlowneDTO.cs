using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SKLEP.Models.Data
{          [Table("RoleKluczeGlowne")]
    public class RoleKluczeGlowneDTO
    {
        [Key, Column(Order = 0)]     //order dla rol. czyli kluczy glownych
        public int UzytkownikID { get; set; }
        [Key, Column(Order = 1)]
        public int RolaID { get; set; }


        [ForeignKey("UzytkownikID")]
        public virtual UzytkownikDTO User { get; set; }
        [ForeignKey("RolaID")]
        public virtual RoleDTO Role { get; set; }

    }
}