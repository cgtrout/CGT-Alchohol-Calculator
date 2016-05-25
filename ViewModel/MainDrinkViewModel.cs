using BloodAlcoholCalculator.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace BloodAlcoholCalculator.ViewModel
{
     public class MainDrinkViewModel : WorkspaceViewModel
     {
          public ConsumedDrinkListViewModel ConsumedDrinkListVM { get; set; }
          public DataRepository<UserViewModel, Model.User> UserRepository { get; set; }
          private UserViewModel _user = null;
          private DateTime _startTime;
          private DispatcherTimer timer;
          private RelayCommand _selectNowCommand;

          public MainDrinkViewModel()
          {
               base.DisplayName = "Main Drink Window";
               ConsumedDrinkListVM = new ConsumedDrinkListViewModel();
               UserRepository = DataRepository<UserViewModel, Model.User>.Instance;

               timer = new DispatcherTimer();
               timer.Interval = new TimeSpan(0, 0, 1);
               timer.Tick += Timer_Tick;

          }

          private void Timer_Tick(object sender, EventArgs e)
          {
               OnPropertyChanged("BloodAlcohol");
          }

          public UserViewModel User
          {
               get {
                    return _user;
               }
               set {
                    _user = value;
                    OnPropertyChanged("User");
                    StartTime = value.StartTime;
                    timer.Start();
                    ConsumedDrinkListVM.LinkDrinks();
                    ConsumedDrinkListVM.User = value;
               }
          }

          public DateTime StartTime {
               get {
                    return _startTime;
               }
               set {
                    _startTime = value;
                    User.StartTime = value;
                    UserRepository.Edit(User);
                    OnPropertyChanged("StartTime");
               }
          }
          
          public double BloodAlcohol
          {
               get {
                    if (User == null) return 0;

                    var deltaTime = DateTime.Now - User.StartTime;
                    OnPropertyChanged("BloodAlcoholString");
                    return CalculationUnit.CalculationUnit.CalculateBac(ConsumedDrinkListVM.GetFilteredList(), User.BaseUser, deltaTime);
               }
          }

          public string BloodAlcoholString => BloodAlcohol.ToString("0.##");

          //SelectNowCommand
          public ICommand SelectNowCommand
          {
               get {
                    if (_selectNowCommand == null) {
                         _selectNowCommand = new RelayCommand(SelectNow);
                    }
                    return _selectNowCommand;
               }
          }

          private void SelectNow(object obj)
          {
               if (User == null) return;
               StartTime = DateTime.Now;
          }


     }
}
