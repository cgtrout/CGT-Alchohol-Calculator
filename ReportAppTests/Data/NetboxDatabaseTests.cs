using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportApp.Data.Tests
{
     [TestClass()]
     public class NetboxDatabaseTests
     {
          #region Methods

          [TestMethod()]
          public void MergeListToRollCallTest()
          {
               var db = new NetboxDatabase(@"c:\CTApp\DB\Test\Data.sqlite");
               db.ClearTableRollCall();

               //first test that exception does not occur
               var inputDict = new Dictionary<string, RollCall>();
               inputDict.Add("_pa", new RollCall() { RollCallId = 1, PersonId = "_pa" });
               inputDict.Add("_pb", new RollCall() { RollCallId = 2, PersonId = "_pb" });
               inputDict.Add("_pc", new RollCall() { RollCallId = 3, PersonId = "_pc" });
               inputDict.Add("_pd", new RollCall() { RollCallId = 4, PersonId = "_pd" });

               db.MergeListToTable(inputDict);
               db.MergeListToTable(inputDict);

               //now add new elem
               inputDict.Add("_pe", new RollCall() { RollCallId = 5, PersonId = "_pe" });
               db.MergeListToTable(inputDict);
               Assert.IsTrue(db.GetContext().RollCalls.Count() == 5, "count should be 5");

               //now remove elem
               inputDict.Remove("_pb");
               db.MergeListToTable(inputDict);
               Assert.IsTrue(db.GetContext().RollCalls.Count() == 4, "count should be 4");
               Assert.IsFalse(db.GetContext().RollCalls.Any(x => x.PersonId == "_pb"), "should not contain person _pb");
               var list = db.GetContext().RollCalls.ToList();
          }

          //[TestMethod()]
          public void ResetAccessEntries()
          {
               var db = new NetboxDatabase(@"c:\CTApp\DB\Data.sqlite");
               var context = db.GetContext();
               var query = from x in context.AccessEntries
                           where x.DtTm > DateTime.Now.Date
                           select x;

               foreach (var entry in query) {
                    entry.ShiftEntryProcessed = false;
               }

               context.SubmitChanges();
          }

          [TestMethod()]
          public void AddFakeNames()
          {
               NetboxDatabase db = new NetboxDatabase(PathSettings.Default.DatabasePath);

               //add fake names
               for (int i = 0; i < 100000; i++) {
                    var id = $"_t_{i}";
                    var p = new Person() {
                         LastModified = DateTime.Today.AddDays(-10),
                         OrientationNumber = i,
                         LastName = $"TEST_DATA_LAST_{i}",
                         FirstName = $"TEST_DATA_LAST_{i}",

                         ExpirationDate = DateTime.Now.AddYears(1),
                         PersonId = id,
                         IsNetbox = true
                    };
                    db.GetContext().People.InsertOnSubmit(p);
               }
               db.GetContext().SubmitChanges();
          }

          #endregion Methods
     }
}