/*
 * Created by SharpDevelop.
 * User: umwr
 * Date: 06/06/2015
 * Time: 01:28
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace BloodAlcoholCalculator
{
     /// <summary>
     /// Description of Startup.
     /// </summary>
     public class Startup
     {
          #region Methods

          [STAThread]
          public static void Main(string[] args)
          {
               //try {
               App app = new App();
               app.Run();
               //}
               //catch (TimeoutException) {
               //     MessageBox.Show("Can not run more than one instance of BloodAlcoholCalculator at a time.  Please close any running instances of BloodAlcoholCalculator.", "Error");
               //}
               //catch (Exception e) {
               //     MessageBox.Show("Program has shut down unexpectedly.  You will need to restart it.");
               //     Trace.TraceError($"Program has failed {e.Message}");
               //     TraceEx.PrintLog(e.StackTrace);
               //     App.Current.MainWindow.Close();
               //}
          }

          #endregion Methods
     }
}