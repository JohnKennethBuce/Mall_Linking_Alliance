using System;
using System.Xml.Serialization;
using Tarsier.Database.Attributes;
using Tarsier.Database.Enums;


namespace Mall_Linking_Alliance.Model
{
    [DbTable("tblsalesline")]
    public class TblSalesLine
    {
        [DbColumn(ColumnType = ColType.Text, NotNull = false)]
        [XmlAttribute("ReceiptNo")]
        public string ReceiptNo { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = false)]
        [XmlAttribute("Sku")]
        public string Sku { get; set; }

        [DbColumn(ColumnType = ColType.Integer, NotNull = false)]
        [XmlAttribute("Qty")]
        public decimal? Qty { get; set; }

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("UnitPrice")]
        public decimal? UnitPrice { get; set; }

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Disc")]
        public decimal? Disc { get; set; }

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Senior")]
        public decimal? Senior { get; set; }

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Pwd")]
        public decimal? Pwd { get; set; }

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Diplomat")]
        public decimal? Diplomat { get; set; }

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("TaxType")]
        public decimal? TaxType { get; set; }

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Tax")]
        public decimal? Tax { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = false)]
        [XmlAttribute("Memo")]
        public string Memo { get; set; }

        [DbColumn(ColumnType = ColType.Numeric, NotNull = false)]
        [XmlAttribute("Total")]
        public decimal? Total { get; set; }
    }
}
