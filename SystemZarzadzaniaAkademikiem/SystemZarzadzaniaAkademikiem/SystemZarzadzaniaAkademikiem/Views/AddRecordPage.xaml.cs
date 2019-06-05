using System;
using SystemZarzadzaniaAkademikiem.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SystemZarzadzaniaAkademikiem.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddRecordPage : ContentPage
	{
        AddRecordViewModel viewModel;
        string tableName;
		public AddRecordPage (string tableName)
		{
            this.tableName = tableName;
			InitializeComponent ();
            BindingContext = viewModel = new AddRecordViewModel(tableName, App.Database);
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadColumnsCommand.Execute(null);
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}