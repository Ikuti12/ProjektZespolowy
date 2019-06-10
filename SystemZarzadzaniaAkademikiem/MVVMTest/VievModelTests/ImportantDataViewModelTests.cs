using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemZarzadzaniaAkademikiem.Data;
using SystemZarzadzaniaAkademikiem.Models;
using SystemZarzadzaniaAkademikiem.Services;
using SystemZarzadzaniaAkademikiem.ViewModels;

namespace MVVMTest.VievModelTests
{
    public class ImportantDataViewModelTests
    {
        private ImportantDataViewModel viewModel;
        private AppDatabase database = new AppDatabase("test");
        private User user = new User();
        private UserRepo userRep;

        [SetUp]
        public void setUp()
        {
            userRep = new UserRepo(database);
            user.Index = "120081";
            user.Name = "Paulina";
            user.Lastname = "Sobczak";
            userRep.SaveUserAsync(user);
            viewModel = new ImportantDataViewModel(database, user);
            viewModel.Sex = SystemZarzadzaniaAkademikiem.Enums.Sex.Woman;
        }

        [Test]
        public void CheckIndexField()
        {
            Assert.AreEqual(viewModel.Index, "120081");
        }

        [Test]
        public void CheckNameField()
        {
            Assert.AreEqual(viewModel.Name, "Paulina");
        }

        [Test]
        public void CheckLastNameField()
        {
            Assert.AreEqual(viewModel.Lastname, "Sobczak");
        }

        //[Test]
        //public void IndexErrorNotFound()
        //{
        //    user.Index = "312312";
        //    viewModel = new ImportantDataViewModel(database, user);
        //    viewModel.SaveImportantDataPreferences.Execute(null);

        //    Assert.AreEqual("Imię nie pasuje do podanego indeksu", viewModel.LastnameError);
        //}

        [Test]
        public void NameErrorNotFound()
        {
            user.Name = "Magda";
            viewModel = new ImportantDataViewModel(database, user);
            viewModel.SaveImportantDataPreferences.Execute(null);

            Assert.AreEqual("Imię nie pasuje do podanego indeksu", viewModel.NameError);
        }

        [Test]
        public void LastNameErrorNotFound()
        {
            user.Lastname = "Nowak";
            viewModel = new ImportantDataViewModel(database, user);
            viewModel.SaveImportantDataPreferences.Execute(null);

            Assert.AreEqual("Nazwisko nie pasuje do podanego indeksu", viewModel.LastnameError);
        }
    }
}
