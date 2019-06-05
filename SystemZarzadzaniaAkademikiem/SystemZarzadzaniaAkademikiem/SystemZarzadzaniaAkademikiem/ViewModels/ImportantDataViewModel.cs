using SystemZarzadzaniaAkademikiem.Data;
using SystemZarzadzaniaAkademikiem.Enums;
using SystemZarzadzaniaAkademikiem.Models;
using SystemZarzadzaniaAkademikiem.Services;
using SystemZarzadzaniaAkademikiem.Validators;
using Xamarin.Forms;

namespace SystemZarzadzaniaAkademikiem.ViewModels
{
    public class ImportantDataViewModel : BaseViewModel
    {
        private string _index = "";
        private string _indexError = "";
        private string _lastname = "";
        private string _lastnameError = "";
        private string _name = "";
        private string _nameError = "";
        public bool isValid;

        public UserRepo userRepo;

        public ImportantDataViewModel(AppDatabase database,User user = null)
        {
            _name = user?.Name;
            _lastname = user?.Lastname;
            _index = user?.Index;
            SaveImportantDataPreferences = new Command(ExecuteSaveImportantDataPreferences);
            userRepo = new UserRepo(database);
        }

        public Command SaveImportantDataPreferences { get; set; }
        private bool NoRoomAssigned()
        {
            if (userRepo.GetUserAsync(Index).Result == null || userRepo.GetUserAsync(Index).Result.RoomNumber!=0)
            {
                return false;
            }
            return true;
        }
        private async void ExecuteSaveImportantDataPreferences()
        {
            isValid = Validate() && Exists() && NoRoomAssigned();
            if (isValid)
            {
                var user = userRepo.GetUserAsync(Index).Result;
                user.Sex = Sex;
                await userRepo.SaveUserAsync(user);
            }
            else if (!Validate())
            {
                if (!ValidateName())
                {
                    Name = Name;
                }
                if (!ValidateLastname())
                {
                    Lastname = Lastname;
                }
                if (!ValidateIndex())
                {
                    Index = Index;
                }
            }
            else if (!Exists())
            {
                if(!NameMatches())
                {
                    NameError ="Imię nie pasuje do podanego indeksu";
                }
                if (!IndexExists())
                {
                    IndexError = "Podany indeks nie znajduje się w bazie danych";
                }
                if (!LastnameMatches())
                {
                    LastnameError = "Nazwisko nie pasuje do podanego indeksu";
                }
            }
            else if (!NoRoomAssigned())
            {
                    NameError = "Masz juz przydzielony pokój, w razie błędów proszę powiadomić obsługę";
                    IndexError = "";
                    LastnameError = "";
            }
        }

        #region

        public Sex Sex { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
                NameError = !ValidateName() ? "Pole nie może być puste" : "";
            }
        }

        private bool ValidateName()
        {
            return Validator.EmptyField(Name);
        }

        public string NameError
        {
            get => _nameError;
            set
            {
                _nameError = value;
                OnPropertyChanged();
            }
        }

        public string Lastname
        {
            get => _lastname;
            set
            {
                _lastname = value;
                OnPropertyChanged();
                LastnameError = !ValidateLastname() ? "Pole nie może być puste" : "";
            }
        }

        private bool ValidateLastname()
        {
            return Validator.EmptyField(Lastname);
        }

        public string LastnameError
        {
            get => _lastnameError;
            set
            {
                _lastnameError = value;
                OnPropertyChanged();
            }
        }

        public string Index
        {
            get => _index;
            set
            {
                _index = value;
                OnPropertyChanged();
                IndexError = !ValidateIndex() ? "Wpisz poprawny indeks" : "";
            }
        }

        private bool ValidateIndex()
        {
            return Validator.IndexVal(Index);
        }

        public string IndexError
        {
            get => _indexError;
            set
            {
                _indexError = value;
                OnPropertyChanged();
            }
        }

        private bool Validate()
        {
            return ValidateName() && ValidateLastname() && ValidateIndex() && ValidateSex();
        }
        private bool ValidateSex()
        {
            return Sex != null;
        }
        private bool Exists()
        {
            return IndexExists() && NameMatches() && LastnameMatches();
        }
        private bool IndexExists()
        {
            return userRepo.GetUserAsync(Index).Result!=null;
        }
        private bool NameMatches()
        {
            if(userRepo.GetUserAsync(Index).Result == null)
            {
                return false;
            }
            return userRepo.GetUserAsync(Index).Result.Name == Name;
        }
        private bool LastnameMatches()
        {
            if (userRepo.GetUserAsync(Index).Result == null)
            {
                return false;
            }
            return userRepo.GetUserAsync(Index).Result.Lastname == Lastname;
        }
        #endregion
    }
}