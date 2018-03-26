using System;
using System.Threading.Tasks;

using Xamarin.Forms;

using AuthApp.Services;


namespace AuthApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        IAuthenticationService _authService;

        const string SIGNIN_TEXT = "Sign In";
        const string SIGNOUT_TEXT = "Sign Out";
        const string SIGNIN_COLOR = "";
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

        Command _signInOutCommand;
        public Command SignInOutCommand
        {
            get => _signInOutCommand;
            set => SetProperty(ref _signInOutCommand, value);
        }

        public SettingsViewModel(IAuthenticationService authService)
        {
            _authService = authService;

            InitCommands();

            // Get initial token/profile state.
            Task.Run(async () =>
            {
                var token = await _authService.GetToken();

                ToggleSignInStatus(token);

                var profile = await _authService.GetUserProfileAsync();

                ToggleProfileInfo(profile);

            });

            _authService.AuthenticationChanged += AuthChanged;
        }

        void InitCommands()
        {
            SignInOutCommand = new Command(async () =>
            {
                var token = await _authService.GetToken();

                if (string.IsNullOrEmpty(token))
                {
                    token = await _authService.GetToken(true);
                }
                else
                {
                    await _authService.SignOut();
                    // Token should be null/empty; but getting it just to keep the UI in sync with the values.
                    token = await _authService.GetToken();
                }

                ToggleSignInStatus(token);

            });
        }

        private void AuthChanged(string token)
        {
            ToggleSignInStatus(token);

            Task.Run(async () =>
            {
                var profile = await _authService.GetUserProfileAsync();

                ToggleProfileInfo(profile);
            });
        }

        void ToggleSignInStatus(string token)
        {
            if (string.IsNullOrEmpty(token))
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

        private void ToggleProfileInfo(UserProfile profile)
        {
            if(profile == null)
            {
                Name = null;
                Email = null;
            }
            else
            {
                Name = profile.Name;
                Email = profile.EmailAddress;
            }
        }
    }
}
