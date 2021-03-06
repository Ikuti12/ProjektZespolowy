﻿using System;
using SystemZarzadzaniaAkademikiem.ViewModels;
using Xamarin.Forms;

namespace SystemZarzadzaniaAkademikiem.Views
{
	public partial class Match : ContentPage
    {
        private MatchViewModel viewModel;

        public Match (MatchViewModel matchViewModel)
		{
			InitializeComponent ();
            BindingContext = viewModel = matchViewModel;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.DecideWhatToDo();
        }
        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            await Navigation.PopToRootAsync();
        }
        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}