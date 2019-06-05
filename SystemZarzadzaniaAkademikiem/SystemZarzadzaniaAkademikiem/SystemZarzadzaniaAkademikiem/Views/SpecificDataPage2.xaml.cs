using System;
using SystemZarzadzaniaAkademikiem.ViewModels;
using Xamarin.Forms;

namespace SystemZarzadzaniaAkademikiem.Views
{
    public partial class SpecificDataPage2 : ContentPage
	{
        private SpecificDataPage2ViewModel viewModel;
        public SpecificDataPage2 (SpecificDataPage2ViewModel specificDataPage2ViewModel)
		{
			InitializeComponent ();
            BindingContext = viewModel = specificDataPage2ViewModel;
        }
        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            if (viewModel.isValid)
            {
                await Navigation.PushAsync(new Match(new MatchViewModel(viewModel.index, App.Database)));
            }
        }
    }
}