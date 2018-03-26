using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AuthApp.Services
{
    public class NavigationService : INavigationService
    {
        private NavigationPage CurrentNavigationPage;

        public NavigationService()
        {
        }

        public void SetRootNavigationPage(NavigationPage navigationPage)
        {
            CurrentNavigationPage = navigationPage;
        }

        public async Task GoBack()
        {
            await CurrentNavigationPage?.PopAsync();
        }

        public async Task NavigateAsync(string pageName, bool animated = true)
        {
            await CurrentNavigationPage?.Navigation.PushAsync(GetPage(pageName, null), animated);
        }

        public async Task NavigateAsync(string pageName, object parameter, bool animated = true)
        {
            await CurrentNavigationPage?.Navigation.PushAsync(GetPage(pageName, parameter), animated);
        }

        public async Task NavigateModalAsync(string pageName, bool animated = true)
        {
            await CurrentNavigationPage?.Navigation.PushModalAsync(GetPage(pageName, null), animated);
        }

        public async Task NavigateModalAsync(string pageName, object parameter, bool animated = true)
        {
            await CurrentNavigationPage?.Navigation.PushModalAsync(GetPage(pageName, parameter), animated);
        }

        private Page GetPage(string pageName, object parameter)
        {
            object[] parameters;

            if (parameter == null)
            {
                parameters = new object[] { };
            }
            else
            {
                parameters = new[] { parameter };
            }

            Xamarin.Forms.Page page = (Xamarin.Forms.Page)Activator.CreateInstance(GetPageType(pageName), parameters);

            return page;
        }

        private Type GetPageType(string pageName)
        {
            string _namespace = CurrentNavigationPage?.CurrentPage.GetType().Namespace;
            string _assembly = CurrentNavigationPage?.CurrentPage.GetType().Assembly.GetName().Name;

            Type type = Type.GetType($"{_namespace}.{pageName}, {_assembly}");

            return type;
        }
    }
}
