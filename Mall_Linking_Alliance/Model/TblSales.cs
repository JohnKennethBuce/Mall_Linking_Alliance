using System;
using System.Xml.Serialization;
using Tarsier.Database.Enums;

namespace Mall_Linking_Alliance.Model
{
    public class TblSales
    {
        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        [XmlAttribute("ReceiptNo")]
        public string ReceiptNo { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        [XmlAttribute("Date")]
        public string Date { get; set; }

        [DbColumn(ColumnType = ColType.Integer, NotNull = true)]
        [XmlAttribute("Void")]
        public int Void { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("Cash")]
        public decimal Cash { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("Credit")]
        public decimal Credit { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("Charge")]
        public decimal Charge { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("GiftCheck")]
        public decimal GiftCheck { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("OtherTender")]
        public decimal OtherTender { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("LineDisc")]
        public decimal LineDisc { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("LineSenior")]
        public decimal LineSenior { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("LinePwd")]
        public decimal LinePwd { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("Evat")]
        public decimal Evat { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("LineDiplomat")]
        public decimal LineDiplomat { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("Subtotal")]
        public decimal Subtotal { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("Disc")]
        public decimal Disc { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("Senior")]
        public decimal Senior { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("Pwd")]
        public decimal Pwd { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("Diplomat")]
        public decimal Diplomat { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("Vat")]
        public decimal Vat { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("ExVat")]
        public decimal ExVat { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("IncVat")]
        public decimal IncVat { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("LocalTax")]
        public decimal LocalTax { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("Amusement")]
        public decimal Amusement { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("Ewt")]
        public decimal Ewt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("Service")]
        public decimal Service { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("TaxSale")]
        public decimal TaxSale { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("NoTaxSale")]
        public decimal NoTaxSale { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("TaxExSale")]
        public decimal TaxExSale { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("TaxIncSale")]
        public decimal TaxIncSale { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("ZeroSale")]
        public decimal ZeroSale { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("VatExempt")]
        public decimal VatExempt { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Integer, NotNull = true)]
        [XmlAttribute("CustomerCount")]
        public int CustomerCount { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("Gross")]
        public decimal Gross { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("TaxRate")]
        public decimal TaxRate { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Long, NotNull = true)]
        [XmlAttribute("Posted")]
        public long Posted { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Text, NotNull = false)]
        [XmlAttribute("Memo")]
        public string Memo { get; set; }

        [DbColumn(ColumnType = ColType.Integer, NotNull = true)]
        [XmlAttribute("Qty")]
        public int Qty { get; set; } = 0;
    }
}
