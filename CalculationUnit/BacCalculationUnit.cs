using BloodAlcoholCalculator.Model;
using System;
using System.Collections.Generic;

namespace BloodAlcoholCalculator.CalculationUnit
{
     public static class BacCalculationUnit
     {
          public static double CalculateBac(List<ConsumedDrink> drinkList, User user, TimeSpan deltaTime)
          {
               double drinks = 0;

               //TODO  possibly check times
               //TODO ensure doesn't drop below zero
               foreach (var d in drinkList) {
                    drinks += CalculateDrinkSize(d.LinkedDrink.Volume, d.LinkedDrink.Percentage);
               }
               var bodyWaterConstant = (user.Male == true) ? Constants.BodyWaterConstantMale : Constants.BodyWaterConstantFemale;
               double bac = ((Constants.WaterInBlood * drinks * 1.2) / (user.BodyWeight * bodyWaterConstant)) - (user.MetabolismFactor * deltaTime.TotalHours);
               return bac;
          }

          public static double CalculateDrinkSize(double volumeMl, double alcoholPercentage)
          {
               return (volumeMl * (alcoholPercentage / 100)) / 12.7;
          }
     }
}