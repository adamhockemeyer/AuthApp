using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Microsoft.Identity.Client;
using Autofac.Core;
using Xamarin.Forms;

using FFImageLoading.Forms.Droid;
using FFImageLoading.Svg.Forms;
using Plugin.VersionTracking;

using AuthApp.Services;


namespace AuthApp.Droid
{
    [Activity(Label = "AuthApp.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Forms.SetFlags("FastRenderers_Experimental");

            global::Xamarin.Forms.Forms.Init(this, bundle);

            // FFImageLoading Init
            CachedImageRenderer.Init(true);
            var ignore = typeof(SvgCachedImage);

            CrossVersionTracking.Current.Track();

            App app = new App(new IModule[] { new PlatformSpecificModule() });

            LoadApplication(app);

            // Set the UIParent for the MSAL Authentication Library.
            ((AuthenticationService)app.GetAuthenticationService).SetUIParent(new UIParent(this));


        }
    }
}
