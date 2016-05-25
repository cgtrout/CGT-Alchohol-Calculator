using System;

namespace BloodAlcoholCalculator.Data
{
     public sealed class SqliteAttribute : Attribute
     {
          public string DataType { get; set; }
          public bool IsPrimaryKey { get; set; } = false;
          public bool IsAutoIncrement { get; set; } = false;
          public bool IsUnique { get; set; } = false;
     }
}