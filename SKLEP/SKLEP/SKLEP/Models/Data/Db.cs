using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SKLEP.Models.Data
{
    public class Db : DbContext // db ze Stringa dodanego :) To jest kontekst
    {
        public DbSet<StronaDTO> Strony { get; set; }// akces poprzez entity framework
        public DbSet<PanelBocznyDTO> Boczny { get; set; }
        public DbSet<KategorieDTO> Kategorie { get; set; }
        public DbSet<ProduktyDTO> Produkty { get; set; }
        public DbSet<UzytkownikDTO> Uzytkownik { get; set; }
        public DbSet<RoleDTO> Rola { get; set; }
        public DbSet<RoleKluczeGlowneDTO> RolaKlucze { get; set; }
        public DbSet<ZamowieniaDTO> Zamowienia { get; set; }
        public DbSet<ZamowieniaSzczegolyDTO> ZamowieniaSzczegoly { get; set; }
        public DbSet<StopkaDTO> Stopka { get; set; }

    }
}
/*
 * connectionStrings>
    <add name="Db" connectionString="Server=(localdb)\MSSQLLocalDb;Integrated Security=true;AttachDbFileName=|DataDirectory|\BazaSklepu.mdf;" providerName="System.Data.SqlClient" />
  </connectionStrings>
*/