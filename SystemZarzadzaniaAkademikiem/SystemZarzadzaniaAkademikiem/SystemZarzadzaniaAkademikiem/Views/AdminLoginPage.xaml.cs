using SystemZarzadzaniaAkademikiem.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SystemZarzadzaniaAkademikiem.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AdminLoginPage : ContentPage
	{
        AdminLoginViewModel viewModel;
		public AdminLoginPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new AdminLoginViewModel(App.Database);
		}
    }
}