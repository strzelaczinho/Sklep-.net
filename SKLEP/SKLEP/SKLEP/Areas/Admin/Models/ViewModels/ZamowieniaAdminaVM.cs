using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SKLEP.Areas.Admin.Models.ViewModels
{
    public class ZamowieniaAdminaVM
    {
        public int OrderNumber { get; set; }
        public string Username { get; set; }
        public decimal Total { get; set; }
        public Dictionary<string, int> ProductsAndQty { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}