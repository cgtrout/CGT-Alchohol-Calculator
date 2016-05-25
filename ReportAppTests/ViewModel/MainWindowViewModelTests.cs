using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportApp.Data;
using ReportApp.Model;
using ReportApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportApp.ViewModel.Tests
{
     [TestClass()]
     public class MainWindowViewModelTests
     {
          #region Methods

          [TestMethod()]
          public void TestMessengingSystem()
          {
               //make copy of db just incase
               //System.IO.File.Copy(PathSettings.Default.DatabasePath, @"c:\_export\_DB Backup\Data.sqlite", true);
               //System.IO.File.Copy(PathSettings.Default.VehicleDatabasePath, @"c:\_export\_DB Backup\Vehicle.sqlite", true);

               //ReportAppMain.InitializeReportApp();
               //var mainWindowVM = new MainWindowViewModel();

               //use test names (undelete them)

               List<Person> testNames = new List<Person>() {
                    new Person() {PersonId="_851", LastName="TEST", FirstName="Test1", Company="Test" },
                    new Person() {PersonId="_795", LastName="TEST", FirstName="Test2", Company="Test" },
                    new Person() {PersonId="_796", LastName="TEST", FirstName="Test3", Company="Test" },
                    new Person() {PersonId="_797", LastName="TEST", FirstName="Test4", Company="Test" },
                    new Person() {PersonId="_798", LastName="TEST", FirstName="Test5", Company="Test" }
               };

               foreach (var p in testNames) {
                    //var t = API_Interaction.ModifyPerson(p, false);
                    //Task.WaitAll(t);
               }

               //open accessvm
               AccessEntriesViewModel accessVM = new AccessEntriesViewModel();

               //subscribe to message
               var messageHandler = DataRepository.AccessLogMessageHandler;

               //create fake access entry changes messages
               //Even calling from one task causes dispatcher begininvoke
               //to not be called
               //var task = Task.Factory.StartNew(() => {
               //object obj = new object();
               var tasks = new Task[20];
               for (int i = 0, y = 0; i < 20; ++i) {
                    var tuple = Tuple.Create(i, y);
                    tasks[i] = Task.Factory.StartNew(t => {
                         var innerTuple = (Tuple<int, int>)t;
                         int innerI = innerTuple.Item1;
                         int innerY = innerTuple.Item2;
                         var args = new CollectionChangedEventArgs();
                         AccessEntry e = new AccessEntry();
                         e.DtTm = DateTime.Now;
                         e.LogId = innerI;
                         e.PersonId = testNames[innerY].PersonId;
                         args.ChangedValue = e;
                         //lock(obj)
                         //problem here is that we end up getting thread
                         //dispatcher instead of main dispatcher
                         //making this test methodology invalid
                         DispatcherHelper.GetDispatcher().BeginInvoke(new Action(() => {
                              messageHandler.OnCollectionChanged(args);
                         }), System.Windows.Threading.DispatcherPriority.Render);
                    }, tuple);

                    y++;
                    if (y == 5) {
                         y = 0;
                    }
               }
               //});
               Task.WaitAll(tasks);

               DispatcherHelper.GetDispatcher().Invoke(new Action(() => {
                    int i = 0;
                    i = i + 1;
               }), System.Windows.Threading.DispatcherPriority.Render);
               DispatcherHelper.GetDispatcher().Invoke(new Action(() => {
               }), System.Windows.Threading.DispatcherPriority.Render);
               DispatcherHelper.GetDispatcher().Invoke(new Action(() => {
               }), System.Windows.Threading.DispatcherPriority.Render);
               DispatcherHelper.GetDispatcher().Invoke(new Action(() => {
               }), System.Windows.Threading.DispatcherPriority.Render);
               DispatcherHelper.GetDispatcher().Invoke(new Action(() => {
               }), System.Windows.Threading.DispatcherPriority.Render);
               DispatcherHelper.GetDispatcher().Invoke(new Action(() => {
               }), System.Windows.Threading.DispatcherPriority.Render);

               //Task.WaitAll(Task.Delay(20000));

               //verify
               for (int i = 0; i < 20; ++i) {
                    Assert.IsTrue(accessVM.AccessEntries.Any(x => x.LogId == i), $"Passes entry {i} not found");
               }

               //open vehiclevm
               //VehicleEntriesViewModel vehicleVM = new VehicleEntriesViewModel(PathSettings.Default.VehicleDatabasePath);

               //open rollcall
               //RollCallViewModel rollcallVM = new RollCallViewModel(false);

               //somehow "inject" fake access log data
               //- try to relicate multiple people signing in at same time
               //if we subscribe to events and place messages, we can test
               //that messages all arrive

               //verify all information is in all vm s

               //delete fake data from db
               //delete testnames
          }

          #endregion Methods
     }
}