using System;
using System.Threading.Tasks;
using AuthApp.Services;
using Xamarin.Forms;

namespace AuthApp.ViewModels
{
    public class ProvideFeedbackViewModel : BaseViewModel
    {
        IAuthenticationService _authService;
        INavigationService _navService;

        string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        bool _isNameFieldEnabled;
        public bool IsNameFieldEnabled
        {
            get => _isNameFieldEnabled;
            private set => SetProperty(ref _isNameFieldEnabled, value);
        }

        string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        bool _isEmailFieldEnabled;
        public bool IsEmailFieldEnabled
        {
            get => _isEmailFieldEnabled;
            private set => SetProperty(ref _isEmailFieldEnabled, value);
        }

        string _positiveFeedback;
        public string PositiveFeedback
        {
            get => _positiveFeedback;
            set => SetProperty(ref _positiveFeedback, value);
        }

        string _negativeFeedback;
        public string NegativeFeedback
        {
            get => _negativeFeedback;
            set => SetProperty(ref _negativeFeedback, value);
        }

        Command _submitFeedbackCommand;
        public Command SubmitFeedbackCommand
        {
            get => _submitFeedbackCommand;
            set => SetProperty(ref _submitFeedbackCommand, value);
        }

        public ProvideFeedbackViewModel(IAuthenticationService authService, INavigationService navService)
        {
            _authService = authService;
            _navService = navService;

            Name = _authService?.Name;
            Email = _authService?.UserId;

            IsNameFieldEnabled = string.IsNullOrEmpty(Name);
            IsEmailFieldEnabled = string.IsNullOrEmpty(Email);

            InitCommands();
        }

        void InitCommands()
        {
            SubmitFeedbackCommand = new Command(async () =>
            {
                if(string.IsNullOrEmpty(PositiveFeedback) || string.IsNullOrEmpty(NegativeFeedback))
                {
                    Acr.UserDialogs.UserDialogs.Instance.Alert("Please provide some feedback before submitting.", "Incomplete");
                }
                else
                {
                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Submitting Feedback (fake)...");

                    // TODO: Call your backend service and submit/save the user provided feedback.
                    await Task.Delay(750);

                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();

                    await _navService.GoBack();
                }
            });
        }
    }
}
