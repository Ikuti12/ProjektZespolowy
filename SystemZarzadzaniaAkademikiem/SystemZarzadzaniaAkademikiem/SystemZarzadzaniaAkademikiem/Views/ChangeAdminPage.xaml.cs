using System;
using SystemZarzadzaniaAkademikiem.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SystemZarzadzaniaAkademikiem.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChangeAdminPage : ContentPage
	{
        AdminChangeViewModel viewModel;
        CRUDMainPage mainPage;
        public ChangeAdminPage (CRUDMainPage mainPage =null)
		{
            this.mainPage = mainPage;
			InitializeComponent ();
            BindingContext = viewModel = new AdminChangeViewModel(App.Database);
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            if (viewModel.isValid)
            {
                await mainPage.NavigateFromMenu(1);
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.ClearData();
        }
    }
}