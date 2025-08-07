using System;
using System.Xml.Serialization;
using Tarsier.Database.Enums;

namespace Mall_Linking_Alliance.Model
{
    public class TblEod
    {
        [DbColumn(ColumnType = ColType.Text, NotNull = false)]
        [XmlAttribute("Date")]
        public string Date { get; set; }

        [DbColumn(ColumnType = ColType.Integer, NotNull = false)]
        [XmlAttribute("ZCounter")]
        public int? ZCounter { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("PreviousNrgt")]
        public decimal PreviousNrgt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Nrgt")]
        public decimal Nrgt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("PreviousTax")]
        public decimal PreviousTax { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("NewTax")]
        public decimal NewTax { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("PreviousTaxSale")]
        public decimal PreviousTaxSale { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("NewTaxSale")]
        public decimal NewTaxSale { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("PreviousNoTaxSale")]
        public decimal PreviousNoTaxSale { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("NewNoTaxSale")]
        public decimal NewNoTaxSale { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Integer, NotNull = false)]
        [XmlAttribute("OpenTime")]
        public int OpenTime { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Integer, NotNull = false)]
        [XmlAttribute("CloseTime")]
        public int CloseTime { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Gross")]
        public decimal Gross { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Vat")]
        public decimal Vat { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("LocalTax")]
        public decimal LocalTax { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Amusement")]
        public decimal Amusement { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Ewt")]
        public decimal Ewt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("TaxSale")]
        public decimal TaxSale { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("NoTaxSale")]
        public decimal NoTaxSale { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("ZeroSale")]
        public decimal ZeroSale { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("VatExempt")]
        public decimal VatExempt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Void")]
        public decimal Void { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Integer, NotNull = false)]
        [XmlAttribute("VoidCnt")]
        public int VoidCnt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Disc")]
        public decimal Disc { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Integer, NotNull = false)]
        [XmlAttribute("DiscCnt")]
        public int DiscCnt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Refund")]
        public decimal Refund { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Integer, NotNull = false)]
        [XmlAttribute("RefundCnt")]
        public int RefundCnt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Senior")]
        public decimal Senior { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Integer, NotNull = false)]
        [XmlAttribute("SeniorCnt")]
        public int SeniorCnt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Pwd")]
        public decimal Pwd { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Integer, NotNull = false)]
        [XmlAttribute("PwdCnt")]
        public int PwdCnt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Diplomat")]
        public decimal Diplomat { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Integer, NotNull = false)]
        [XmlAttribute("DiplomatCnt")]
        public int DiplomatCnt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Service")]
        public decimal Service { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Integer, NotNull = false)]
        [XmlAttribute("ServiceCnt")]
        public int ServiceCnt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Text, NotNull = false)]
        [XmlAttribute("ReceiptStart")]
        public string ReceiptStart { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = false)]
        [XmlAttribute("ReceiptEnd")]
        public string ReceiptEnd { get; set; }

        [DbColumn(ColumnType = ColType.Integer, NotNull = false)]
        [XmlAttribute("TrxCnt")]
        public int TrxCnt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Cash")]
        public decimal Cash { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Integer, NotNull = false)]
        [XmlAttribute("CashCnt")]
        public int CashCnt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Credit")]
        public decimal Credit { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Integer, NotNull = false)]
        [XmlAttribute("CreditCnt")]
        public int CreditCnt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Charge")]
        public decimal Charge { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Integer, NotNull = false)]
        [XmlAttribute("ChargeCnt")]
        public int ChargeCnt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("GiftCheck")]
        public decimal GiftCheck { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Integer, NotNull = false)]
        [XmlAttribute("GiftCheckCnt")]
        public int GiftCheckCnt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("OtherTender")]
        public decimal OtherTender { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Integer, NotNull = false)]
        [XmlAttribute("OtherTenderCnt")]
        public int OtherTenderCnt { get; set; } = 0;
    }
}
