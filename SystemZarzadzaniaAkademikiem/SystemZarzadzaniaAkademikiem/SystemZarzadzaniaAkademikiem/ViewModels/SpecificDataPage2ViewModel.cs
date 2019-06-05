using SystemZarzadzaniaAkademikiem.Data;
using SystemZarzadzaniaAkademikiem.Enums;
using SystemZarzadzaniaAkademikiem.Models;
using SystemZarzadzaniaAkademikiem.Services;
using Xamarin.Forms;

namespace SystemZarzadzaniaAkademikiem.ViewModels
{
    public class SpecificDataPage2ViewModel : BaseViewModel
    {
        public readonly string index;
        private readonly User user;
        private readonly UserRepo userRepo;
        public bool isValid = false;


        public SpecificDataPage2ViewModel(string index, AppDatabase database)
        {
            user = new User();
            this.index = index;
            SaveSpecificData2 = new Command(ExecuteSaveSpecificData2);
            userRepo = new UserRepo(database);
        }

        public Command SaveSpecificData2 { get; set; }

        private async void ExecuteSaveSpecificData2()
        {
            isValid = Validate();
            if (isValid)
            {
                var user = userRepo.GetUserAsync(index).Result;
                user.Talkative = Talkative;
                user.StudyField = StudyField;
                user.Sporting = Sporting;
                user.HomeBack = HomeBack;
                user.Smoking = Smoking;
                user.Party = Party;
                await userRepo.SaveUserAsync(user);
            }
        }

        public bool Validate()
        {
            return ValidateHomeBack() && ValidateTalkative() && ValidateSporting() && ValidateStudyField() &&
                ValidateParty() && ValidateSmoking();
        }
        #region
        public HomeBack HomeBack { get; set; } = HomeBack.Empty;

        private bool ValidateHomeBack()
        {
            return HomeBack != HomeBack.Empty;
        }


        public Talkative Talkative { get; set; } = Talkative.Empty;

        private bool ValidateTalkative()
        {
            return Talkative != Talkative.Empty;
        }


       public YesNo Sporting { get; set; } = YesNo.Empty;

        private bool ValidateSporting()
        {
            return Sporting != YesNo.Empty;
        }


        public StudyField StudyField { get; set; } = StudyField.Empty;

        private bool ValidateStudyField()
        {
            return StudyField != StudyField.Empty;
        }

        public Party Party { get; set; } = Party.Empty;

        private bool ValidateParty()
        {
            return Party != Party.Empty;
        }

        public YesNo Smoking { get; set; } = YesNo.Empty;

        private bool ValidateSmoking()
        {
            return Smoking != YesNo.Empty;
        }


        #endregion
    }
}