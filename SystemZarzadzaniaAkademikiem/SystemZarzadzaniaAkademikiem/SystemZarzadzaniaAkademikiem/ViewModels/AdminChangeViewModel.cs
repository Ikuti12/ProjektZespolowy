using SystemZarzadzaniaAkademikiem.Services;
using Xamarin.Forms;
using SystemZarzadzaniaAkademikiem.Validators;
using SystemZarzadzaniaAkademikiem.Data;

namespace SystemZarzadzaniaAkademikiem.ViewModels
{
    public class AdminChangeViewModel : BaseViewModel
    {
        private string _newLogin;
        private string _newPassword;
        private string _newLoginError;
        private string _newPasswordError;
        public bool isValid = false;
        public string NewLogin
        {
            get
            {
                return _newLogin;
            }
            set
            {
                _newLogin = value;
                OnPropertyChanged();
            }
        }
        public string NewPassword
        {
            get
            {
                return _newPassword;
            }
            set
            {
                _newPassword = value;
                OnPropertyChanged();
            }
        }
        public string NewPasswordError
        {
            get
            {
                return _newPasswordError;
            }
            set
            {
                _newPasswordError = value;
                OnPropertyChanged();
            }
        }
        public string NewLoginError
        {
            get
            {
                return _newLoginError;
            }
            set
            {
                _newLoginError = value;
                OnPropertyChanged();
            }
        }
        private bool Validate()
        {
            return ValidPassword() && ValidLogin();
        }
        private bool ValidPassword()
        {
            return Validator.ValidLogin(NewPassword);
        }
        private bool ValidLogin()
        {
            return Validator.ValidLogin(NewLogin);
        }
        public Command ChangeAdmin { get; set; }
        AdminRepo adminRepo;
        private AppDatabase database;
        public AdminChangeViewModel(AppDatabase database)
        {
            Title = "Changing Admin Credentials";
            ChangeAdmin = new Command(ExecuteChangeAdmin);
            adminRepo = new AdminRepo(database);
        }
        public void ClearData()
        {
            NewPassword = "";
            NewLogin = "";
            NewLoginError = "";
            NewPasswordError = "";
        }
        async void ExecuteChangeAdmin()
        {
            NewPasswordError = "";
            NewLoginError = "";
            isValid = Validate();
            if (isValid)
            {
                await adminRepo.SaveAdminAsync(new Models.SuperUser { Id = 1, Login = NewLogin, Password = NewPassword });
            }
            else
            {
                if (!ValidPassword())
                {
                    NewPasswordError = "To hasło jest za słabe! Wymyśl bardziej bezpieczne hasło";
                }
                if (!ValidLogin())
                {
                    NewLoginError = "Ten login jest za krótki! Wymyśl dłuższy login";

                }
            }
        }
    }
}
