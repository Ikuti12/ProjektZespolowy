using System;
using SystemZarzadzaniaAkademikiem.ViewModels;
using Xamarin.Forms;

namespace SystemZarzadzaniaAkademikiem.Views
{
	public partial class SpecificDataPage1 : ContentPage
	{
        private SpecificDataPage1ViewModel viewModel;

        public SpecificDataPage1 (SpecificDataPage1ViewModel specificDataPage1ViewModel)
		{
            BindingContext = viewModel = specificDataPage1ViewModel;
            InitializeComponent ();
		}
        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            if (viewModel.isValid)
            {
                await Navigation.PushAsync(new SpecificDataPage2(new SpecificDataPage2ViewModel(viewModel.index, App.Database)));
            }
        }
    }
}