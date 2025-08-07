using System;
using System.Data.Entity.Core.Metadata.Edm;
using Tarsier.Database.Enums;

namespace Mall_Linking_Alliance.Model
{
    #region Attributes
    /// <summary>
    /// Use this attribute to decorate the properties on your model class.
    /// Only those properties that are having exactly the same column name of a DB table.
    /// </summary>
    public class DbColumnAttribute : Attribute
    {
        /// <summary>
        /// Set true if implicit conversion is required.
        /// </summary>
        public bool Convert { get; set; }

        /// <summary>
        /// Set true if ID is auto increment
        /// </summary>
        public bool AutoIncrement { get; set; }
        /// <summary>
        /// Indicate column is UNIQUE
        /// </summary>
        /// 
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Denotes if the field is an identity type or not.
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// Set column data type
        /// </summary>
        public ColType ColumnType { get; set; }

        /// <summary>
        /// Indicate column not null
        /// </summary>
        public bool NotNull { get; set; }

        /// <summary>
        /// Set default value
        /// </summary>
        public object DefaultValue { get; set; } = string.Empty;
    }
    #endregion
}