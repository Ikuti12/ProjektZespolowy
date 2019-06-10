using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Windows.Input;
using SystemZarzadzaniaAkademikiem.Data;
using SystemZarzadzaniaAkademikiem.Models;
using SystemZarzadzaniaAkademikiem.Services;
using SystemZarzadzaniaAkademikiem.ViewModels;
using SystemZarzadzaniaAkademikiem.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MVVMTest.VievModelTests
{
    public class AdminLoginViewModelTests
    {
        private AdminLoginViewModel viewModel;
        private AppDatabase database = new AppDatabase("test");

        [SetUp]
        public void Setup()
        {
            viewModel = new AdminLoginViewModel(database);
        }

        [Test]
        public void CheckInputForLogin()
        {
            viewModel.Login = "1234567890";

            Assert.AreEqual("1234567890", viewModel.Login);
        }

        [Test]
        public void CheckInputForPassword()
        {
            viewModel.Password = "qwerty";

            Assert.AreEqual("qwerty", viewModel.Password);
        }

        [Test]
        public void CheckMessageWhenLoginEmpty()
        {
            viewModel.Login = "";
            viewModel.Password = "";
            var result = viewModel.LoginError;

            Assert.AreEqual("Pole nie mo¿e byæ puste", result);
        }

        [Test]
        public void CheckMessageWhenWrongLogin()
        {
            viewModel.Login = "1234567890";
            viewModel.Password = "S3cr3tP@ss";
            viewModel.LoginAsAdmin.Execute(null);

            var result = viewModel.LoginError;

            Assert.AreEqual("Has³o albo login nie s¹ prawid³owe", result);
        }

        [Test]
        public void CheckMessageWhenWrongPassword()
        {
            viewModel.Login = "Admin";
            viewModel.Password = "qwerty";
            viewModel.LoginAsAdmin.Execute(null);

            var result = viewModel.PasswordError;

            Assert.AreEqual("Has³o albo login nie s¹ prawid³owe", result);
        }
    }
}