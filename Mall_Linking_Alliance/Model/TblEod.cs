using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall_Linking_Alliance.Model
{
    public class TblEod
    {
        public string Date { get; set; }
        public int? ZCounter { get; set; } = 0;
        public decimal? PreviousNrgt { get; set; } = 0;
        public decimal? Nrgt { get; set; } = 0;
        public decimal? PreviousTax { get; set; } = 0;
        public decimal? NewTax { get; set; } = 0;
        public decimal? PreviousTaxSale { get; set; } = 0;
        public decimal? NewTaxSale { get; set; } = 0;
        public decimal? PreviousNoTaxSale { get; set; } = 0;
        public decimal? NewNoTaxSale { get; set; } = 0;
        public int OpenTime { get; set; } = 0;
        public int CloseTime { get; set; } = 0;
        public decimal? Gross { get; set; } = 0;
        public decimal? Vat { get; set; } = 0;
        public decimal? LocalTax { get; set; } = 0;
        public decimal? Amusement { get; set; } = 0;
        public decimal? Ewt { get; set; } = 0;
        public decimal? TaxSale { get; set; } = 0;
        public decimal? NoTaxSale { get; set; } = 0;
        public decimal? ZeroSale { get; set; } = 0;
        public decimal? VatExempt { get; set; } = 0;
        public decimal? Void { get; set; } = 0;
        public int? VoidCnt { get; set; } = 0;
        public decimal? Disc { get; set; } = 0;
        public int? DiscCnt { get; set; } = 0;
        public decimal? Refund { get; set; } = 0;
        public int? RefundCnt { get; set; } = 0;
        public decimal? Senior { get; set; } = 0;
        public int? SeniorCnt { get; set; } = 0;
        public decimal? Pwd { get; set; } = 0;
        public int? PwdCnt { get; set; } = 0;
        public decimal? Diplomat { get; set; } = 0;
        public int? DiplomatCnt { get; set; } = 0;
        public decimal? Service { get; set; } = 0;
        public int? ServiceCnt { get; set; } = 0;
        public string ReceiptStart { get; set; }
        public string ReceiptEnd { get; set; }
        public int? TrxCnt { get; set; } = 0;
        public decimal? Cash { get; set; } = 0;
        public int? CashCnt { get; set; } = 0;
        public decimal? Credit { get; set; } = 0;
        public int? CreditCnt { get; set; } = 0;
        public decimal? Charge { get; set; } = 0;
        public int? ChargeCnt { get; set; } = 0;
        public decimal? GiftCheck { get; set; } = 0;
        public int? GiftCheckCnt { get; set; } = 0;
        public decimal? OtherTender { get; set; } = 0;
        public int? OtherTenderCnt { get; set; } = 0;
    }
}
