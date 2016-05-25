using BloodAlcoholCalculator.Data;
using System.Data.Linq.Mapping;

namespace BloodAlcoholCalculator.Model
{
     /// <summary>
     /// A user defined drink
     /// </summary>
     [Table(Name = "DefinedDrink")]
     public class DefinedDrink
     {
          [Column(Name = "Id", IsPrimaryKey = true, UpdateCheck = UpdateCheck.Never)]
          [Sqlite(DataType = "INTEGER", IsPrimaryKey = true, IsAutoIncrement = true, IsUnique = true)]
          public long Id { get; set; }

          [Column(Name = "Name", UpdateCheck = UpdateCheck.Never)]
          [Sqlite(DataType = "TEXT")]
          public string Name { get; set; }

          [Column(Name = "Percentage", UpdateCheck = UpdateCheck.Never)]
          [Sqlite(DataType = "REAL")]
          public double Percentage { get; set; }

          [Column(Name = "Volume", UpdateCheck = UpdateCheck.Never)]
          [Sqlite(DataType = "REAL")]
          public double Volume { get; set; }
     }
}