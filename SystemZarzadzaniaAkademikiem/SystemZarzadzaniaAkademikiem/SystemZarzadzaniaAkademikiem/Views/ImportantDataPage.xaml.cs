using System;
using SystemZarzadzaniaAkademikiem.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SystemZarzadzaniaAkademikiem.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ImportantDataPage : ContentPage
	{
        private ImportantDataViewModel viewModel;
        public ImportantDataPage ()
		{
			InitializeComponent ();
            viewModel = new ImportantDataViewModel(App.Database);
            BindingContext = viewModel;
        }

        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            if (viewModel.isValid)
            {
                await Navigation.PushAsync(new SpecificDataPage1(new SpecificDataPage1ViewModel(viewModel.Index, App.Database)));
            }
        }
    }
}