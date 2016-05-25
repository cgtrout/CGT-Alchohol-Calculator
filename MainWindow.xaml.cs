/*
 * Created by SharpDevelop.
 * User: umwr
 * Date: 2015-05-29
 * Time: 9:09 PM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using BloodAlcoholCalculator.Console;
using BloodAlcoholCalculator.Utility;
using System;
using System.Windows;

namespace BloodAlcoholCalculator
{
     /// <summary>
     /// Interaction logic for Window1.xaml
     /// </summary>
     public partial class MainWindow : Window
     {
          //private readonly DispatcherTimer clockTimer;

          public MainWindow()
          {
               InitializeComponent();
               TraceEx.PrintLog("Starting application");
               WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

               var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
               DateTime buildDate = new DateTime(2000, 1, 1)
                    .AddDays(version.Build)
                    .AddSeconds(version.Revision * 2);
               Title = String.Format("BA Calculator: Version {1} ({0})", buildDate.ToString("D"), Version.Default.VersionString);

               ConsoleSystem consoleSystem = ConsoleSystem.ConsoleSystemInstance;
               //consoleSystem.AddCommand(new ConsoleCommand("Quit", "Quit the application", CloseApplication));
          }

          private void clockTimer_Tick(object sender, EventArgs e)
          {
               TextBoxTime.Text = DateTime.Today.ToString("dddd MMMM-dd | ") + DateTime.Now.ToString("HH:mm:ss");
          }
     }
}