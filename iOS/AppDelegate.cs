using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using Autofac.Core;
using FFImageLoading.Forms.Touch;
using FFImageLoading.Svg.Forms;

namespace AuthApp.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            // FFImageLoading Init
            CachedImageRenderer.Init();
            var ignore = typeof(SvgCachedImage);

            LoadApplication(new App(new IModule[] { new PlatformSpecificModule() }));

            return base.FinishedLaunching(app, options);
        }

		public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
		{
            Microsoft.Identity.Client.AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
            return true;
		}
	}
}
