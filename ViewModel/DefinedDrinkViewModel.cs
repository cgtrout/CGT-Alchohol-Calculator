using BloodAlcoholCalculator.Model;
using BloodAlcoholCalculator.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodAlcoholCalculator.ViewModel
{
     public class DefinedDrinkViewModel : ViewModelBase, IComparable<DefinedDrinkViewModel>, ViewModelConvertor<DefinedDrinkViewModel, DefinedDrink>
     {
          public DefinedDrink Drink { get; set; } = new DefinedDrink();
          
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

          public string Name
          {
               get {
                    return Drink.Name;
               }
               set {
                    Drink.Name = value;
                    OnPropertyChanged("Name");
               }
          }

          public double Percentage
          {
               get { return Drink.Percentage; }
               set {
                    Drink.Percentage = value;
                    OnPropertyChanged("Percentage");
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

          public DefinedDrinkViewModel ConvertFrom(DefinedDrink type)
          {
               Drink = type;
               return this;
          }

          public DefinedDrink GetBaseType()
          {
               return Drink;
          }

          public int CompareTo(DefinedDrinkViewModel other)
          {
               return this.Name.CompareTo(other.Name);
          }

          public long GetBaseId()
          {
               return Drink.Id;
          }
     }
}
