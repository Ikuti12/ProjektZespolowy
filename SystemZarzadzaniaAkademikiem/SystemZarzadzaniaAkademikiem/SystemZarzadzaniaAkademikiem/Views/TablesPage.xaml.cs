using SystemZarzadzaniaAkademikiem.Models;
using SystemZarzadzaniaAkademikiem.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SystemZarzadzaniaAkademikiem.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TablesPage : ContentPage
	{
        TablesViewModel viewModel;
		public TablesPage ()
		{
			InitializeComponent ();
            BindingContext =viewModel = new TablesViewModel(App.Database);
		}
        async void OnTableSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var table = args.SelectedItem as Table;
            if (table == null)
                return;
            await Navigation.PushAsync(new TableDetailPage(new TableDetailViewModel(table.name, App.Database), App.Database));

            // Manually deselect item.
            TablesListView.SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadTablesCommand.Execute(null);
        }
    }
}