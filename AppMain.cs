//using BloodAlcoholCalculator.Model;
using BloodAlcoholCalculator.Utility;
using BloodAlcoholCalculator.ViewModel;
using System;

namespace BloodAlcoholCalculator
{
     public static class AppMain
     {
          #region Methods

          public static void InitializeBloodAlcoholCalculator()
          {
               //for some reason this has to be loaded here or application
               //will close before window opens
               TraceEx.PrintLog("Loading MainWindow");
               var viewModel = MainWindowViewModel.MainWindowInstance;
               MainWindow window = new MainWindow();

               // When the ViewModel asks to be closed,
               // close the window.
               EventHandler handler = null;
               handler = delegate {
                    viewModel.RequestClose -= handler;

                    //call dispose to ensure it is always closed
                    viewModel.Dispose();
                    window.Close();
                    //window.WindowState = WindowState.Minimized;
               };
               viewModel.RequestClose += handler;
               window.DataContext = viewModel;
               TraceEx.PrintLog("Window.Show()");
               window.Show();
          }

          #endregion Methods
     }
}