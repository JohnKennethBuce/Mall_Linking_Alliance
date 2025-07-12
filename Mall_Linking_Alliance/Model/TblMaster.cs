using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall_Linking_Alliance.Model
{
    public class TblMaster
    {
        public string ReceiptNo { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public int? Inventory { get; set; } = 0;
        public decimal? Price { get; set; } = 0;
        public string Category { get; set; }
    }
}
