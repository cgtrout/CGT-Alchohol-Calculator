using BloodAlcoholCalculator.Data;
using System;
using System.Data.Linq.Mapping;

namespace BloodAlcoholCalculator.Model
{
     [Table(Name = "User")]
     public class User
     {
          [Column(Name = "Id", IsPrimaryKey = true, UpdateCheck = UpdateCheck.Never)]
          [Sqlite(DataType = "INTEGER", IsPrimaryKey = true, IsAutoIncrement = true, IsUnique = true)]
          public long Id { get; set; }

          [Column(Name = "Name", UpdateCheck = UpdateCheck.Never)]
          [Sqlite(DataType = "TEXT")]
          public string Name { get; set; }

          [Column(Name = "BodyWeight", UpdateCheck = UpdateCheck.Never)]
          [Sqlite(DataType = "INTEGER")]
          public int BodyWeight { get; set; }

          [Column(Name = "MetabolismFactor", UpdateCheck = UpdateCheck.Never)]
          [Sqlite(DataType = "REAL")]
          public double MetabolismFactor { get; set; } = Constants.MetabolismFactorMale;

          [Column(Name = "DesiredMaxBac", UpdateCheck = UpdateCheck.Never)]
          [Sqlite(DataType = "REAL")]
          public double DesiredMaxBac { get; set; } = 0.08;

          [Column(Name = "Male", UpdateCheck = UpdateCheck.Never)]
          [Sqlite(DataType = "INTEGER")]
          public bool Male { get; set; } = true;

          [Column(Name = "StartTime", UpdateCheck = UpdateCheck.Never)]
          [Sqlite(DataType = "TEXT")]
          public DateTime StartTime { get; set; }
     }
}