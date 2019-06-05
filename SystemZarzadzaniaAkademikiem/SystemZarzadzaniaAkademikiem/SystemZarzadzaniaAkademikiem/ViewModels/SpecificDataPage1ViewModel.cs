using SystemZarzadzaniaAkademikiem.Data;
using SystemZarzadzaniaAkademikiem.Enums;
using SystemZarzadzaniaAkademikiem.Models;
using SystemZarzadzaniaAkademikiem.Services;
using Xamarin.Forms;

namespace SystemZarzadzaniaAkademikiem.ViewModels
{
    public class SpecificDataPage1ViewModel : BaseViewModel
    {
        public readonly string index;
        private readonly User user;
        private readonly UserRepo userRepo;
        public bool isValid = false;

        public SpecificDataPage1ViewModel(string index, AppDatabase database)
        {
            user = new User();
            this.index = index;
            SaveSpecificData1 = new Command(ExecuteSaveSpecificData1);
            userRepo = new UserRepo(database);
        }

        public Command SaveSpecificData1 { get; set; }

        private async void ExecuteSaveSpecificData1()
        {
            isValid = Validate();
            if (isValid)
            {
                var user = userRepo.GetUserAsync(index).Result;
                user.Floor = Floor;
                user.BedLocation = BedLocation;
                user.SleepTime = SleepTime;
                user.WakeUpTime = WakeUpTime;
                user.HotOrNot = HotOrNot;
                user.Music = Music;
                user.CleanUp = CleanUp;
                await userRepo.SaveUserAsync(user);
            }
        }

        public bool Validate()
        {
            return ValidateBedLocation() && ValidateCleanUp() && ValidateFloor() && ValidateHotOrNot() && ValidateMusic() &&
                ValidateSleepTime() && ValidateWakeUpTime();
        }
        #region
        public BedLocation BedLocation { get; set; } = BedLocation.Empty;
        public string BedLocationError { get; set; } = "";
        private bool ValidateBedLocation()
        {
            return BedLocation != BedLocation.Empty;
        }

        public YesNo CleanUp { get; set; } = YesNo.Empty;

        private bool ValidateCleanUp()
        {
            return CleanUp != YesNo.Empty;
        }


        public Floor Floor { get; set; } = Floor.Empty;

        private bool ValidateFloor()
        {
            return Floor != Floor.Empty;
        }


        public HotOrNot HotOrNot { get; set; } = HotOrNot.Empty;

        private bool ValidateHotOrNot()
        {
            return HotOrNot != HotOrNot.Empty;
        }


        public Music Music { get; set; } = Music.Empty;

        private bool ValidateMusic()
        {
            return Music != Music.Empty;
        }

        public SleepTime SleepTime { get; set; } = SleepTime.Empty;

        private bool ValidateSleepTime()
        {
            return SleepTime != SleepTime.Empty;
        }

        public WakeUpTime WakeUpTime { get; set; } = WakeUpTime.Empty;

        private bool ValidateWakeUpTime()
        {
            return WakeUpTime != WakeUpTime.Empty;
        }


        #endregion
    }
}