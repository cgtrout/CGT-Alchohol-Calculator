using System;
using System.Windows;

namespace BloodAlcoholCalculator
{
     /// <summary>
     /// Interaction logic for App.xaml
     /// </summary>
     public partial class App : Application
     {
          public App()
          {
               AppDomain currentDomain = AppDomain.CurrentDomain;
          }

          protected override void OnStartup(StartupEventArgs e)
          {
               base.OnStartup(e);

               AppMain.InitializeBloodAlcoholCalculator();
          }
     }
}