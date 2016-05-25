using API_Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportApp.Data;
using System.Threading.Tasks;

namespace ReportApp.ViewModel.Tests
{
     [TestClass()]
     public class AddPersonViewModelTests
     {
          #region Methods

          [TestMethod()]
          public async Task TestCredentialChange()
          {
               long fobNumber = 44654;
               long pinNumber = 9999;
               //delete names to ensure data is clean
               await API_Interaction.RemovePerson("_1044");
               await API_Interaction.RemovePerson("_1068");

               var p1 = new PersonViewModel(await API_Interaction.LoadSinglePerson("_1044"));
               var p2 = new PersonViewModel(await API_Interaction.LoadSinglePerson("_1068"));
               DataRepository.AddPerson(p1);
               DataRepository.AddPerson(p2);

               AddPersonViewModel vm1 = AddPersonViewModel.Create(p1, true);
               AddPersonViewModel vm2 = AddPersonViewModel.Create(p2, true);

               //add fake fob to vm1
               vm1.CurrentPerson.FobNumber = fobNumber;
               vm1.CurrentPerson.PinNumber = pinNumber;
               await vm1.Save();

               //check vm1 information exists on vm1
               var checkPerson = await API_Interaction.LoadSinglePerson("_1044");
               Assert.IsTrue(checkPerson.FobNumber == fobNumber);
               Assert.IsTrue(checkPerson.PinNumber == pinNumber);

               //add same fob to vm2
               vm2.CurrentPerson.FobNumber = fobNumber;
               vm2.CurrentPerson.PinNumber = pinNumber;
               await vm2.Save();

               //check fake fob exists on vm
               var checkPerson2 = await API_Interaction.LoadSinglePerson("_1068");
               Assert.IsTrue(checkPerson2.FobNumber == fobNumber);
               Assert.IsTrue(checkPerson2.PinNumber == pinNumber);

               //check fob does not exist on vm1
               var checkPerson3 = await API_Interaction.LoadSinglePerson("_1044");
               Assert.IsTrue(checkPerson3.FobNumber == 0);
               Assert.IsTrue(checkPerson3.PinNumber == 0);

               //delete names (clean up)
               await API_Interaction.RemovePerson("_1044");
               await API_Interaction.RemovePerson("_1068");
          }

          #endregion Methods
     }
}