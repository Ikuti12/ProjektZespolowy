using SystemZarzadzaniaAkademikiem.Data;
using SystemZarzadzaniaAkademikiem.Services;
using SystemZarzadzaniaAkademikiem.Validators;
using SystemZarzadzaniaAkademikiem.Views;
using Xamarin.Forms;

namespace SystemZarzadzaniaAkademikiem.ViewModels
{
    public class AdminLoginViewModel : BaseViewModel
    {
        private readonly AdminRepo adminRepo;
        private string _login;
        private string _password;
        private string _loginError;
        private string _passwordError;

        public AdminLoginViewModel(AppDatabase database)
        {
            Title = "Welcome to Admin Login Page.Please enter credentials below.";
            LoginAsAdmin = new Command(ExecuteLoginAsAdmin);
            adminRepo = new AdminRepo(database);
        }

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
                LoginError = !ValidateLogin() ? "Pole nie może być puste" : "";
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                PasswordError = !ValidatePassword() ? "Pole nie może być puste" : "";
            }
        }
        private bool ValidatePassword()
        {
            return Validator.EmptyField(Password);
        }
        private bool ValidateLogin()
        {
            return Validator.EmptyField(Login);
        }
        public string PasswordError
        {
            get => _passwordError;
            set
            {
                _passwordError = value;
                OnPropertyChanged();
            }
        }
        public string LoginError
        {
            get => _loginError;
            set
            {
                _loginError = value;
                OnPropertyChanged();
            }
        }
        public Command LoginAsAdmin { get; set; }

        private async void ExecuteLoginAsAdmin()
        {
            if (_login != adminRepo.GetAdmin().Result.Login || _password != StringCipher.Decrypt(adminRepo.GetAdmin().Result.Password, adminRepo.GetAdmin().Result.Salt))
            {
                    PasswordError = "Hasło albo login nie są prawidłowe";
                    LoginError = "Hasło albo login nie są prawidłowe";
            }
            else
            {
                await Application.Current.MainPage.Navigation.PushAsync(new CRUDMainPage());
            }
        }
    }
}