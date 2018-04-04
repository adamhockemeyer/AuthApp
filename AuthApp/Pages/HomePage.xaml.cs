using System;
using System.Collections.Generic;

using Xamarin.Forms;

using AuthApp.ViewModels;

namespace AuthApp.Pages
{
    public partial class HomePage : BaseContentPage<HomeViewModel>
    {
        public HomePage()
        {
            InitializeComponent();

            // Hides the Navigation Bar on the main page
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }

        public void OnToggleMenuTapped(object sender, EventArgs args)
        {
            //Navigation.
            if(Application.Current.MainPage is MasterDetailPage)
            {
                ((MasterDetailPage)Application.Current.MainPage).IsPresented = !((MasterDetailPage)Application.Current.MainPage).IsPresented;
            }
        }
	}
}
