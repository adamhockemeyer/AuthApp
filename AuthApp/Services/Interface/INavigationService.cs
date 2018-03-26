using System;

using System.Threading.Tasks;

namespace AuthApp.Services
{
    public interface INavigationService
    {

        Task GoBack();
        Task NavigateModalAsync(string pageName, bool animated = true);
        Task NavigateModalAsync(string pageName, object parameter, bool animated = true);
        Task NavigateAsync(string pageName, bool animated = true);
        Task NavigateAsync(string pageName, object parameter, bool animated = true);
    }
}
