using System;
using System.Xml.Serialization;
using Tarsier.Database.Enums;

namespace Mall_Linking_Alliance.Model
{
    public class TblMaster
    {
        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        [XmlAttribute("Sku")]
        public string Sku { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [DbColumn(ColumnType = ColType.Integer, NotNull = true)]
        [XmlAttribute("Inventory")]
        public int Inventory { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Numeric, NotNull = true)]
        [XmlAttribute("Price")]
        public decimal Price { get; set; } = 0;

        [DbColumn(ColumnType = ColType.Text, NotNull = false)]
        [XmlAttribute("Category")]
        public string Category { get; set; }
    }
}
