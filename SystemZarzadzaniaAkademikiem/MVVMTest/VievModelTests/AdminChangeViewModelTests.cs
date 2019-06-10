using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemZarzadzaniaAkademikiem.Data;
using SystemZarzadzaniaAkademikiem.ViewModels;

namespace MVVMTest.VievModelTests
{
    public class AdminChangeViewModelTests
    {
        private AdminChangeViewModel viewModel;
        private AppDatabase database = new AppDatabase("test");

        [SetUp]
        public void Setup()
        {
            viewModel = new AdminChangeViewModel(database);
        }

        [Test]
        public void ChangeLogin()
        {
            viewModel.NewLogin = "1234567890";

            Assert.AreEqual("1234567890", viewModel.NewLogin);
        }

        [Test]
        public void ChangePassword()
        {
            viewModel.NewPassword = "qwerty";

            Assert.AreEqual("qwerty", viewModel.NewPassword);
        }

        [Test]
        public void ErrorWhenNotValidationPassword()
        {
            viewModel.ChangeAdmin.Execute(null);

            Assert.AreEqual("To hasło jest za słabe! Wymyśl bardziej bezpieczne hasło", viewModel.NewPasswordError);
        }

        [Test]
        public void ErrorWhenNotValidationLogin()
        {
            viewModel.ChangeAdmin.Execute(null);

            Assert.AreEqual("Ten login jest za krótki! Wymyśl dłuższy login", viewModel.NewLoginError);
        }

        [Test]
        public void EmptyErrorMessageLoginWhenSuccefullChange()
        {
            viewModel.NewLogin = "Basia12";
            viewModel.NewPassword = "P@s$w0rd";
            viewModel.ChangeAdmin.Execute(null);

            Assert.AreEqual(string.Empty, viewModel.NewLoginError);
        }

        [Test]
        public void EmptyErrorMessagePasswordWhenSuccefullChange()
        {
            viewModel.NewLogin = "Basia12";
            viewModel.NewPassword = "P@s$w0rd";
            viewModel.ChangeAdmin.Execute(null);

            Assert.AreEqual(string.Empty, viewModel.NewPasswordError);
        }

        [Test]
        public void ClearDataLogin()
        {
            viewModel.NewLogin = "Basia12";
            viewModel.NewPassword = "P@s$w0rd";
            viewModel.ClearData();

            Assert.AreEqual(string.Empty, viewModel.NewLogin);
        }

        [Test]
        public void ClearDataPassword()
        {
            viewModel.NewLogin = "Basia12";
            viewModel.NewPassword = "P@s$w0rd";
            viewModel.ClearData();

            Assert.AreEqual(string.Empty, viewModel.NewPassword);
        }
    }
}
