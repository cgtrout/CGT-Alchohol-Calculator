using BloodAlcoholCalculator.Repository;
using System.Windows.Input;
using System;

namespace BloodAlcoholCalculator.ViewModel
{
     public class DefinedDrinkListViewModel : WorkspaceViewModel
     {
          private ICommand _rowEditCommand;
          private ICommand _cellEditCommand;
          DefinedDrinkViewModel _selectedValue;
          private RelayCommand _consumeDrinkCommand;
          private ICommand _addNewCommand;
          private ICommand _deleteDrinkCommand;

          public DataRepository<DefinedDrinkViewModel, Model.DefinedDrink> Repository { get; set; }
          public DataRepository<ConsumedDrinkViewModel, Model.ConsumedDrink> ConsumedDrinkRepository { get; set; }
          public DataRepository<UserViewModel, Model.User> UserRepository { get; set; }

          public DefinedDrinkListViewModel()
          {
               Repository = DataRepository<DefinedDrinkViewModel, Model.DefinedDrink>.Instance;
               ConsumedDrinkRepository = DataRepository<ConsumedDrinkViewModel, Model.ConsumedDrink>.Instance;
               UserRepository = DataRepository<UserViewModel, Model.User>.Instance;

               base.DisplayName = "Drink List";
          }

          public ICommand RowEditCommand
          {
               get {
                    if (_rowEditCommand == null) {
                         _rowEditCommand = new RelayCommand(x => RowEdit());
                    }
                    return _rowEditCommand;
               }
          }

          private void RowEdit()
          {
               Repository.Edit(SelectedValue);
          }

          public ICommand CellEditCommand
          {
               get {
                    if (_cellEditCommand == null) {
                         _cellEditCommand = new RelayCommand(x => CellEdit());
                    }
                    return _cellEditCommand;
               }
          }

          private void CellEdit()
          {
               Repository.Edit(SelectedValue);
          }

          public ICommand ConsumeDrinkCommand
          {
               get {
                    if (_consumeDrinkCommand == null) {
                         _consumeDrinkCommand = new RelayCommand(x => ConsumeDrink());
                    }
                    return _consumeDrinkCommand;
               }
          }

          //AddNewCommand
          public ICommand AddNewCommand
          {
               get {
                    if (_addNewCommand == null) {
                         _addNewCommand = new RelayCommand(x => AddNew());
                    }
                    return _addNewCommand;
               }
          }

          private void AddNew()
          {
               Repository.Add(new DefinedDrinkViewModel() { Name = "New Drink" });
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

          private void ConsumeDrink()
          {
               //add drink for each user
               foreach(var user in UserRepository.Dict.Values) {
                    ConsumedDrinkViewModel consumedDrink = new ConsumedDrinkViewModel()
                    {
                         Time = DateTime.Now,
                         LinkedDrink = SelectedValue.Drink
                    };
                    consumedDrink.Drink.LinkedUserId = user.Id;
                    consumedDrink.Drink.LinkedDrinkId = SelectedValue.Id;
                    ConsumedDrinkRepository.Add(consumedDrink);
               }
          }

          public DefinedDrinkViewModel SelectedValue
          {
               get { return _selectedValue; }
               set {
                    _selectedValue = value;
                    OnPropertyChanged("SelectedValue");
               }
          }
     }
}