using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

using AuthApp.Models;
using AuthApp.Services;

namespace AuthApp.Pages
{
    public partial class MainPage : MasterDetailPage
    {
        INavigationService _navService;

        // Toggle's whether large titles should be used for iOS
        bool _prefersLargeTitlesiOS = true;

        Color _navBarTextColor = Color.White;
        Color _navBarBackgroundColor = (Color)Application.Current.Resources["blueColor"];

        public MainPage(INavigationService navService)
        {
            InitializeComponent();

            this._navService = navService;

            masterPage.ListView.ItemSelected += OnItemSelected;

            // Set the Bar Text and Background Color for the inital page.
            if(Detail is Xamarin.Forms.NavigationPage)
            {
                ((Xamarin.Forms.NavigationPage)Detail).BarTextColor = _navBarTextColor;
                ((Xamarin.Forms.NavigationPage)Detail).BarBackgroundColor = _navBarBackgroundColor;

                // Set the navigation page
                ((NavigationService)_navService).SetRootNavigationPage(Detail as Xamarin.Forms.NavigationPage);

                ((Xamarin.Forms.NavigationPage)Detail).On<iOS>().SetPrefersLargeTitles(_prefersLargeTitlesiOS);
            }

        }

        // Updates the selected page to display
        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                masterPage.UnsetActive();

                item.Active = true;

                Xamarin.Forms.Page page = (Xamarin.Forms.Page)Activator.CreateInstance(item.TargetType);

                Detail = new Xamarin.Forms.NavigationPage(page)
                {
                    BarTextColor = _navBarTextColor,
                    BarBackgroundColor = _navBarBackgroundColor
                };

                // Set the navigation page
                ((NavigationService)_navService).SetRootNavigationPage(Detail as Xamarin.Forms.NavigationPage);

                ((Xamarin.Forms.NavigationPage)Detail).On<iOS>().SetPrefersLargeTitles(_prefersLargeTitlesiOS);
            }

            masterPage.ListView.SelectedItem = null;
            IsPresented = false;
        }
    }
}
