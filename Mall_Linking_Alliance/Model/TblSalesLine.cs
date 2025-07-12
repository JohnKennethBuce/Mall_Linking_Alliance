using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall_Linking_Alliance.Model
{
    public class TblSalesLine
    {
        public string ReceiptNo { get; set; }
        public string Sku { get; set; }
        public decimal? Qty { get; set; } = 0;
        public decimal? UnitPrice { get; set; } = 0;
        public decimal? Disc { get; set; } = 0;
        public decimal? Senior { get; set; } = 0;
        public decimal? Pwd { get; set; } = 0;
        public decimal? Diplomat { get; set; } = 0;
        public decimal? TaxType { get; set; } = 0;
        public decimal? Tax { get; set; } = 0;
        public string Memo { get; set; } 
        public decimal? Total { get; set; } = 0;



    }
}
