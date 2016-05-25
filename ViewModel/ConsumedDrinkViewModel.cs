using BloodAlcoholCalculator.Model;
using System;

namespace BloodAlcoholCalculator.ViewModel
{
     public class ConsumedDrinkViewModel : ViewModelBase, IComparable<ConsumedDrinkViewModel>, ViewModelConvertor<ConsumedDrinkViewModel, ConsumedDrink>
     {
          public ConsumedDrink Drink { get; set; } = new ConsumedDrink();
          private DefinedDrinkViewModel _linkedDrink;

          public long Id
          {
               get {
                    return Drink.Id;
               }
               set {
                    Drink.Id = value;
                    OnPropertyChanged("Id");
               }
          }

          public DateTime Time
          {
               get {
                    return Drink.Time;
               }
               set {
                    Drink.Time = value;
                    OnPropertyChanged("Time");
               }
          }

          public double Volume
          {
               get {
                    return Drink.Volume;
               }
               set {
                    Drink.Volume = value;
                    OnPropertyChanged("Volume");
               }
          }

          public DefinedDrink LinkedDrink
          {
               get {
                    return Drink.LinkedDrink;
               }
               set {
                    Drink.LinkedDrink = value;
                    OnPropertyChanged("LinkedDrink");
               }
          }

          public int CompareTo(ConsumedDrinkViewModel other)
          {
               return this.Time.CompareTo(other.Time);
          }

          public ConsumedDrinkViewModel ConvertFrom(ConsumedDrink type)
          {
               Drink = type;
               return this;
          }

          public ConsumedDrink GetBaseType()
          {
               return Drink;
          }

          public long GetBaseId()
          {
               return Drink.Id;
          }
     }
}