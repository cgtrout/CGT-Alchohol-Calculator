using API_Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportApp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Interface.Tests
{
     [TestClass()]
     public class API_InteractionTests
     {
          #region Methods

          [TestMethod()]
          public void CreateShiftListTest()
          {
               var accessList = new List<AccessEntry>() {
                    new AccessEntry() {
                         LogId = 1, DtTm = DateTime.Parse("2015-11-18 06:46"),
                         Reader = "Admin IN",
                         PersonId = "1"
                    },
                    new AccessEntry() {
                         LogId = 2, DtTm = DateTime.Parse("2015-11-18 07:50"),
                         Reader = "Admin IN",
                         PersonId = "1"
                    },
                    new AccessEntry() {
                         LogId = 3, DtTm = DateTime.Parse("2015-11-18 16:31"),
                         Reader = "Admin OUT",
                         PersonId = "1"
                    },
               };
               //var shiftList = API_Interaction.CreateShiftList(accessList, new List<ShiftEntry>());
               //shiftList = API_Interaction.CreateShiftList(accessList, shiftList);
               //shiftList = API_Interaction.CreateShiftList(accessList, shiftList);

               //Assert.IsTrue(shiftList.Count == 1);
          }

          //[TestMethod()]
          public async Task LoadPersonDetailsBenchmark()
          {
               Stopwatch sw = new Stopwatch();

               //test 1
               //API_Interaction ai = new API_Interaction();
               sw.Start();
               var personList = await API_Interaction.LoadPersonDetails(DeleteType.False);
               sw.Stop();
               var time1 = sw.ElapsedMilliseconds;

               sw.Reset();

               //test 2
               sw.Start();
               var personList2 = await API_Interaction.LoadPersonDetails(DeleteType.True);
               sw.Stop();
               var time2 = sw.ElapsedMilliseconds;

               var messageText = $"Time1={time1} Time2={time2}";
               using (var textFile = File.CreateText("API Load BenchMark.txt")) {
                    textFile.WriteLine(messageText);

                    textFile.WriteLine("------------List 1");
                    textFile.WriteLine(PrintPersonList(personList));

                    textFile.WriteLine("------------List 2");
                    textFile.WriteLine(PrintPersonList(personList2));
               }

               //Assert.Fail();
          }

          private string PrintPersonList(List<Person> personList)
          {
               StringBuilder sb = new StringBuilder();
               var query = from p in personList
                           orderby p.Deleted, p.LastName, p.FirstName
                           select p;
               int deletedCount = 0;
               foreach (var p in query) {
                    sb.AppendLine(p.ToString());
                    if (p.Deleted == true) {
                         deletedCount++;
                    }
               }
               sb.AppendLine($"Total Count={personList.Count} Deleted Count={deletedCount} NonDeleted Count={personList.Count - deletedCount}");
               return sb.ToString();
          }

          #endregion Methods

          [TestMethod()]
          public void CreateShiftListTestNightShift()
          {
               var accessList = new List<AccessEntry>() {
                    new AccessEntry() {
                         LogId = 1, DtTm = DateTime.Parse("2015-11-18 06:46"),
                         Reader = "Admin IN",
                         PersonId = "1"
                    },
                    new AccessEntry() {
                         LogId = 2, DtTm = DateTime.Parse("2015-11-19 04:45"),
                         Reader = "Admin IN",
                         PersonId = "1"
                    },
                    new AccessEntry() {
                         LogId = 3, DtTm = DateTime.Parse("2015-11-19 04:46"),
                         Reader = "Admin OUT",
                         PersonId = "1"
                    },
               };
               var shiftList = API_Interaction.CreateShiftList(accessList, new List<ShiftEntry>()).Item1;
               //shiftList = API_Interaction.CreateShiftList(accessList, shiftList).Item1;
               //shiftList = API_Interaction.CreateShiftList(accessList, shiftList).Item1;

               Assert.IsTrue(shiftList.Count == 1);
          }

          [TestMethod()]
          public void CheckDuplicates()
          {
               var accessList = new List<AccessEntry>() {
                    new AccessEntry() {
                         LogId = 1, DtTm = DateTime.Parse("2015-11-18 06:46"),
                         Reader = "Admin IN",
                         PersonId = "1"
                    },
                    new AccessEntry() {
                         LogId = 2, DtTm = DateTime.Parse("2015-11-18 07:46"),
                         Reader = "Admin IN",
                         PersonId = "1"
                    },
                    new AccessEntry() {
                         LogId = 3, DtTm = DateTime.Parse("2015-11-19 04:46"),
                         Reader = "Admin OUT",
                         PersonId = "1"
                    },
               };
               var shiftList = API_Interaction.CreateShiftList(accessList, new List<ShiftEntry>()).Item1;
               shiftList = API_Interaction.CreateShiftList(accessList, shiftList).Item1;
               shiftList = API_Interaction.CreateShiftList(accessList, shiftList).Item1;

               Assert.IsTrue(shiftList.Count == 1);
          }

          //[TestMethod]
          ////this isn't a test, but a fragment to capitalize all names in netbox system
          //public async Task CapitalizeAllNames()
          //{
          //     //get names from database
          //     API_Interaction i = new API_Interaction();
          //     var personList = await API_Interaction.LoadPersonDetails(DeleteType.All);
          //     var query = from p in personList
          //                 where p.Deleted == false
          //                 select p;
          //     TextInfo ti = new CultureInfo("en-US", false).TextInfo;
          //     DBLoadStatus.DirectToOutputWindow = true;

          //     foreach (var p in query) {
          //          if (p.LastName == "Administrator") {
          //               continue;
          //          }
          //          p.LastName = ti.ToUpper(p.LastName);
          //          await API_Interaction.ModifyPerson(p, true);
          //     }
          //}

          //[TestMethod]
          ////this isn't a test, but a fragment to capitalize all names in netbox system
          //public async Task EmptyToATCOPower()
          //{
          //     //get names from database
          //     API_Interaction i = new API_Interaction();
          //     var personList = await API_Interaction.LoadPersonDetails(DeleteType.All);
          //     var query = from p in personList
          //                 where p.Deleted == false
          //                 select p;
          //     TextInfo ti = new CultureInfo("en-US", false).TextInfo;
          //     DBLoadStatus.DirectToOutputWindow = true;

          //     foreach (var p in query) {
          //          if (p.LastName == "Administrator") {
          //               continue;
          //          }
          //          if(string.IsNullOrEmpty(p.Company)) {
          //               p.Company = "ATCO Power";
          //          }
          //          await API_Interaction.ModifyPerson(p, true);
          //     }
          //}
     }
}