namespace BloodAlcoholCalculator.Model
{
     public static class Constants
     {
          public static double WaterInBlood { get; set; } = 0.806;
          public static double BodyWaterConstantMale { get; set; } = 0.58;
          public static double BodyWaterConstantFemale { get; set; } = 0.49;

          public static double MetabolismFactorMale { get; set; } = 0.015;
          public static double MetabolismFactorFemale { get; set; } = 0.017;

          public static double OptimalMaxBac { get; set; } = 0.059;
     }
}