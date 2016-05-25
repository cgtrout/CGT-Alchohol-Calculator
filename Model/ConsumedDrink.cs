using BloodAlcoholCalculator.Data;
using System;
using System.Data.Linq.Mapping;

namespace BloodAlcoholCalculator.Model
{
     /// <summary>
     /// Drink that has been consumed
     /// </summary>
     [Table(Name = "ConsumedDrink")]
     public class ConsumedDrink
     {
          [Column(Name = "Id", IsPrimaryKey = true, UpdateCheck = UpdateCheck.Never)]
          [Sqlite(DataType = "INTEGER", IsPrimaryKey = true, IsAutoIncrement = true, IsUnique = true)]
          public long Id { get; set; }

          [Column(Name = "Time", UpdateCheck = UpdateCheck.Never)]
          [Sqlite(DataType = "TEXT")]
          public DateTime Time { get; set; }

          [Column(Name = "LinkedDrinkId", UpdateCheck = UpdateCheck.Never)]
          [Sqlite(DataType = "INTEGER")]
          public long LinkedDrinkId { get; set; }

          [Column(Name = "LinkedUserId", UpdateCheck = UpdateCheck.Never)]
          [Sqlite(DataType = "INTEGER")]
          public long LinkedUserId { get; set; }

          [Column(Name = "Volume", UpdateCheck = UpdateCheck.Never)]
          [Sqlite(DataType = "REAL")]
          public double Volume { get; set; }

          public DefinedDrink LinkedDrink { get; set; }
     }
}