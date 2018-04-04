using System;
using System.Threading.Tasks;

using Xamarin.Forms;

using AuthApp.Services;
using AuthApp.Constants;

namespace AuthApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        INavigationService _navService;
        IAuthenticationService _authService;

        Command _onPageSelectedCommand;
        public Command OnPageSelectedCommand
        {
            get => _onPageSelectedCommand;
            set => SetProperty(ref _onPageSelectedCommand, value);
        }

        Command _signInCommand;
        public Command SignInCommand
        {
            get => _signInCommand;
            set => SetProperty(ref _signInCommand, value);
        }

        public HomeViewModel(INavigationService navService, IAuthenticationService authService)
        {
            _navService = navService;
            _authService = authService;

            InitCommands();

            // Require Sign In right away on the Home 
            SignInCommand.Execute(null);
        }

        void InitCommands()
        {
            // Handles when a page should be navigated to.
            OnPageSelectedCommand = new Command(async (pageName) =>
            {
                // Navigate to the page selected.
                await _navService.NavigateAsync(pageName.ToString());

            });

            SignInCommand = new Command(async () =>
            {
                await _authService.GetToken(Authentication.SCOPES_API,true);
            });
        }


    }
}
