﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SystemZarzadzaniaAkademikiem.Views;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using SystemZarzadzaniaAkademikiem.Models;

namespace SystemZarzadzaniaAkademikiem.ViewModels
{
    class MainPageViewModel : BaseViewModel
    {
        static int MaxCounter = 1;
        public Command ActivateAdminLogin { get; set; }
        public ObservableCollection<User> Users { get; set; }
        public int Counter=0;
        public MainPageViewModel()
        {
            Title = "System przydzielania miejsc w akademiku";
            Users = new ObservableCollection<User>();
            ActivateAdminLogin = new Command(ExecuteActivateAdminLogin);
        }
        async void ExecuteActivateAdminLogin()
        {
            Counter++;
            if (Counter >= MaxCounter)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AdminLoginPage());
            }
            Debug.WriteLine(Counter);
        }

    }
}
