using System;
using System.Threading.Tasks;

using Xamarin.Forms;

using AuthApp.Constants;
using AuthApp.Services;

using Plugin.VersionTracking;

namespace AuthApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        INavigationService _navService;
        IAuthenticationService _authService;

        const string SIGNIN_TEXT = "Sign In";
        const string SIGNOUT_TEXT = "Sign Out";
        const string SIGNIN_COLOR = "#3A93CF";
        const string SIGNOUT_COLOR = "#FF0000";

        string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        string _signInOutText;
        public string SignInOutText
        {
            get => _signInOutText;
            set => SetProperty(ref _signInOutText, value);
        }

        string _signInOutTextColor;
        public string SignInOutTextColor
        {
            get => _signInOutTextColor;
            set => SetProperty(ref _signInOutTextColor, value);
        }

        string _appVersion;
        public string AppVersion
        {
            get => _appVersion;
            private set => SetProperty(ref _appVersion, value);
        }


        Command _signInOutCommand;
        public Command SignInOutCommand
        {
            get => _signInOutCommand;
            set => SetProperty(ref _signInOutCommand, value);
        }

        Command _onPageSelectedCommand;
        public Command OnPageSelectedCommand
        {
            get => _onPageSelectedCommand;
            set => SetProperty(ref _onPageSelectedCommand, value);
        }

        public SettingsViewModel(INavigationService navService, IAuthenticationService authService)
        {
            _navService = navService;
            _authService = authService;

            InitCommands();

            AppVersion = $"{CrossVersionTracking.Current.CurrentVersion} ({CrossVersionTracking.Current.CurrentBuild})";

            // Get initial token/profile state.
            ToggleSignInStatus();
            ToggleProfileInfo();

            _authService.AuthenticationChanged += AuthChanged;
        }

        void InitCommands()
        {
            // Handles when a page should be navigated to.
            OnPageSelectedCommand = new Command(async (pageName) =>
            {
                // Navigate to the page selected.
                await _navService.NavigateAsync(pageName.ToString());

            });

            SignInOutCommand = new Command(async () =>
            {
                var token = await _authService.GetToken(Authentication.SCOPES_API);

                if (string.IsNullOrEmpty(token))
                {
                    token = await _authService.GetToken(Authentication.SCOPES_API, true);
                }
                else
                {
                    await _authService.SignOut();
                    // Token should be null/empty; but getting it just to keep the UI in sync with the values.
                    token = await _authService.GetToken(Authentication.SCOPES_API);
                }

                ToggleSignInStatus();
            });
        }

        private void AuthChanged(string token, string[] scopes)
        {
            ToggleSignInStatus();

            ToggleProfileInfo();
         }

        void ToggleSignInStatus()
        {
            if (string.IsNullOrEmpty(_authService.UserId))
            {
                SignInOutText = SIGNIN_TEXT;
                SignInOutTextColor = SIGNIN_COLOR;
            }
            else
            {
                SignInOutText = SIGNOUT_TEXT;
                SignInOutTextColor = SIGNOUT_COLOR;
            }
        }

        private void ToggleProfileInfo()
        {
            Name = _authService?.Name;
            Email = _authService?.UserId;
        }
    }
}
