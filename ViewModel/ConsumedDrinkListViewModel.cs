using BloodAlcoholCalculator.Repository;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using BloodAlcoholCalculator.Model;
using System;

namespace BloodAlcoholCalculator.ViewModel
{
     public class ConsumedDrinkListViewModel : WorkspaceViewModel
     {
          private ICommand _deleteDrinkCommand;
          private ConsumedDrinkViewModel _selectedValue;
          private ICollectionView _view;
          private UserViewModel _user;

          public DataRepository<ConsumedDrinkViewModel ,Model.ConsumedDrink> Repository { get; set; }
          public DataRepository<DefinedDrinkViewModel, Model.DefinedDrink> DefinedDrinkRepository { get; set; }
          
          public ConsumedDrinkListViewModel()
          {
               Repository = DataRepository<ConsumedDrinkViewModel, Model.ConsumedDrink>.Instance;
               DefinedDrinkRepository = DataRepository<DefinedDrinkViewModel, Model.DefinedDrink>.Instance;
               
               base.DisplayName = "Drink History";

               LinkDrinks();

               View = new CollectionViewSource { Source = Repository.Collection }.View;

               //load nothing before person selected
               View.Filter = item => {
                    return false;
               };
          }

          public ICollectionView View
          {
               get { return _view; }
               set {
                    _view = value;
                    OnPropertyChanged("View");
               }
          }

          public UserViewModel User
          {
               get {
                    return _user;
               }
               set {
                    _user = value;
                    OnPropertyChanged("User");

                    View = new CollectionViewSource { Source = Repository.Collection }.View;
                    View.Filter = item => {
                         return Filter(item);
                    };
               }
          }

          private bool Filter(object item)
          {
               var consumedDrink = item as ConsumedDrinkViewModel;

               if (consumedDrink.Drink.LinkedUserId == User.GetBaseId()) {
                    return true;
               } else {
                    return false;
               }
          }

          
          public IEnumerable<ConsumedDrinkViewModel> GetFilteredQuery()
          {
               return Repository.Collection.Where(Filter);
          }

          public List<ConsumedDrink> GetFilteredList()
          {
               var list = new List<ConsumedDrink>();
               foreach(var v in GetFilteredQuery()) {
                    list.Add(v.Drink);
               }
               return list;
          }

          public void LinkDrinks()
          {
               foreach (var v in Repository.Collection) {
                    if (DefinedDrinkRepository.Dict.ContainsKey(v.Drink.LinkedDrinkId)) {
                         v.LinkedDrink = DefinedDrinkRepository.Dict[v.Drink.LinkedDrinkId].Drink;
                    } else {
                         v.LinkedDrink = null;
                    }
               }
          }

          //DeleteDrink
          public ICommand DeleteDrinkCommand
          {
               get {
                    if (_deleteDrinkCommand == null) {
                         _deleteDrinkCommand = new RelayCommand(x => DeleteDrink());
                    }
                    return _deleteDrinkCommand;
               }
          }

          private void DeleteDrink()
          {
               Repository.Remove(SelectedValue);
          }
                    
          public ConsumedDrinkViewModel SelectedValue
          {
               get { return _selectedValue; }
               set {
                    _selectedValue = value;
                    OnPropertyChanged("SelectedValue");
               }
          }
     }
}