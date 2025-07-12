using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall_Linking_Alliance.Model
{
    public class TblSales
    {
        public string ReceiptNo { get; set; }
        public string Date { get; set; }
        public int? Void { get; set; } = 0;
        public decimal? Cash { get; set; } = 0;
        public decimal? Credit { get; set; } = 0;
        public decimal? Charge { get; set; } = 0;
        public decimal? GiftCheck { get; set; } = 0;
        public decimal? OtherTender { get; set; } = 0;
        public decimal? LineDisc { get; set; } = 0;
        public decimal? LineSenior { get; set; } = 0;
        public decimal? LinePwd { get; set; } = 0;
        public decimal? Evat { get; set; } = 0;
        public decimal? LineDiplomat { get; set; } = 0;
        public decimal? Subtotal { get; set; } = 0;
        public decimal? Disc { get; set; } = 0;
        public decimal? Senior { get; set; } = 0;
        public decimal? Pwd { get; set; } = 0;
        public decimal? Diplomat { get; set; } = 0;
        public decimal? Vat { get; set; } = 0;
        public decimal? ExVat { get; set; } = 0;
        public decimal? IncVat { get; set; } = 0;
        public decimal? LocalTax { get; set; } = 0;
        public decimal? Amusement { get; set; } = 0;
        public decimal? Ewt { get; set; } = 0;
        public decimal? Service { get; set; } = 0;
        public decimal? TaxSale { get; set; } = 0;
        public decimal? NoTaxSale { get; set; } = 0;
        public decimal? TaxExSale { get; set; } = 0;
        public decimal? TaxIncSale { get; set; } = 0;
        public decimal? ZeroSale { get; set; } = 0;
        public decimal? VatExempt { get; set; } = 0;
        public int? CustomerCount { get; set; } = 0;
        public decimal? Gross { get; set; } = 0;
        public decimal? TaxRate { get; set; } = 0;
        public int? Posted { get; set; } = 0;
        public string Memo { get; set; }
        public int? Qty { get; set; } = 0;

    }
}
