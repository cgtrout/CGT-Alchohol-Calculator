using BloodAlcoholCalculator.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BloodAlcoholCalculator.ViewModel
{
     public class EditUsersViewModel : WorkspaceViewModel
     {
          private ICommand _addNewCommand;
          private UserViewModel _selectedValue;
          private ICommand _editCommand;

          public DataRepository<UserViewModel, Model.User> Repository { get; set; }

          public EditUsersViewModel()
          {
               Repository = DataRepository<UserViewModel, Model.User>.Instance;
               base.DisplayName = "Edit Users";
          }

          //AddNewCommand
          public ICommand AddNewCommand
          {
               get {
                    if (_addNewCommand == null) {
                         _addNewCommand = new RelayCommand(AddNew);
                    }
                    return _addNewCommand;
               }
          }

          private void AddNew(object obj)
          {
               var newItem = new UserViewModel();
               newItem.IsEdit = false;
               MainWindowViewModel.MainWindowInstance.ShowView(newItem);
               this.OnRequestClose();
          }

          public UserViewModel SelectedValue
          {
               get { return _selectedValue; }
               set {
                    _selectedValue = value;
                    OnPropertyChanged("SelectedValue");
               }
          }

          public ICommand EditCommand
          {
               get {
                    if (_editCommand == null) {
                         _editCommand = new RelayCommand(Edit);
                    }
                    return _editCommand;
               }
          }

          private void Edit(object obj)
          {
               SelectedValue.IsEdit = true;
               MainWindowViewModel.MainWindowInstance.ShowView(SelectedValue);
               this.OnRequestClose();
          }
     }
}
