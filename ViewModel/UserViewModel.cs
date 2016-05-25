using BloodAlcoholCalculator.Model;
using BloodAlcoholCalculator.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BloodAlcoholCalculator.ViewModel
{
     public class UserViewModel : WorkspaceViewModel, IComparable<UserViewModel>, ViewModelConvertor<UserViewModel, Model.User>
     {
          public User BaseUser = new User();
          private ICommand _saveCommand;
          private ICommand _selectNowCommand;

          public UserViewModel()
          {
               base.DisplayName = "Edit User";
          }

          public bool IsEdit { get; set; } = false;

          public long Id
          {
               get { return BaseUser.Id; }
               set {
                    BaseUser.Id = value;
                    OnPropertyChanged("Id");
               }
          }

          public string Name
          {
               get { return BaseUser.Name; }
               set {
                    BaseUser.Name = value;
                    OnPropertyChanged("Name");
               }
          }

          public int BodyWeight
          {
               get { return BaseUser.BodyWeight; }
               set {
                    BaseUser.BodyWeight = value;
                    OnPropertyChanged("BodyWeight");
               }
          }

          public double MetabolismFactor
          {
               get { return BaseUser.MetabolismFactor; }
               set {
                    BaseUser.MetabolismFactor = value;
                    OnPropertyChanged("MetabolismFactor");
               }
          }

          public double DesiredMaxBac
          {
               get { return BaseUser.DesiredMaxBac; }
               set {
                    BaseUser.DesiredMaxBac = value;
                    OnPropertyChanged("DesiredMaxBac");
               }
          }

          public bool Male
          {
               get { return BaseUser.Male; }
               set {
                    BaseUser.Male = value;
                    OnPropertyChanged("Male");
               }
          }

          public DateTime StartTime
          {
               get { return BaseUser.StartTime; }
               set {
                    BaseUser.StartTime = value;
                    OnPropertyChanged("StartTime");
               }
          }

          //SaveCommand
          public ICommand SaveCommand
          {
               get {
                    if (_saveCommand == null) {
                         _saveCommand = new RelayCommand(Save);
                    }
                    return _saveCommand;
               }
          }

          private void Save(object obj)
          {
               var repos = DataRepository<UserViewModel, Model.User>.Instance;
               if(IsEdit) {
                    repos.Edit(this);
               } else {
                    repos.Add(this);
               }
          }
                    

          public int CompareTo(UserViewModel other)
          {
               return this.Name.CompareTo(other.Name);
          }

          public UserViewModel ConvertFrom(User type)
          {
               this.BaseUser = type;
               return this;
          }

          public User GetBaseType()
          {
               return BaseUser;
          }

          public long GetBaseId()
          {
               return this.Id;
          }
     }
}
