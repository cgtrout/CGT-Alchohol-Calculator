using BloodAlcoholCalculator.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BloodAlcoholCalculator.CalculationUnit
{
     public static class BacCalculationUnit
     {
          public static double CalculateBac(List<ConsumedDrink> drinkList, User user, TimeSpan deltaTime)
          {
               double drinks = GetDrinks(drinkList);
               var bodyWaterConstant = (user.Male == true) ? Constants.BodyWaterConstantMale : Constants.BodyWaterConstantFemale;

               double bac = ((Constants.WaterInBlood * drinks * 1.2) / (user.BodyWeight * bodyWaterConstant)) - (user.MetabolismFactor * deltaTime.TotalHours);
               return bac;
          }

          private static double GetDrinks(List<ConsumedDrink> drinkList)
          {
               double drinks = 0;

               //TODO  possibly check times
               //TODO ensure doesn't drop below zero
               foreach (var d in drinkList)
               {
                    drinks += CalculateDrinkSize(d.LinkedDrink.Volume, d.LinkedDrink.Percentage);
               }

               return drinks;
          }

          public static double CalculateDrinkSize(double volumeMl, double alcoholPercentage)
          {
               return (volumeMl * (alcoholPercentage / 100)) / 12.7;
          }

          public static TimeSpan CalculateTimeToDesiredBac(List<ConsumedDrink> drinkList, User user, TimeSpan deltaTime)
          {
               double drinks = GetDrinks(drinkList);
               var bodyWaterConstant = (user.Male == true) ? Constants.BodyWaterConstantMale : Constants.BodyWaterConstantFemale;
               ConsumedDrink lastDrink = drinkList.Count == 0 ? null : drinkList.Last();
               var lastDrinkSize = lastDrink == null ? 0 : CalculateDrinkSize(lastDrink.LinkedDrink.Volume, lastDrink.LinkedDrink.Percentage);
               double timeToNext = ((((((Constants.WaterInBlood * (drinks + lastDrinkSize) * 1.2) / (user.BodyWeight * bodyWaterConstant)) - user.DesiredMaxBac) / user.MetabolismFactor)) - deltaTime.TotalHours) * 60;
               if(timeToNext < 0)
               {
                    timeToNext = 0;
               }
               return new TimeSpan((long)timeToNext * TimeSpan.TicksPerMinute);
          }
     }
}