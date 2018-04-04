using System;
using System.Collections.Generic;

using Xamarin.Forms;

using AuthApp.ViewModels;
using System.IO;
using System.Reflection;

namespace AuthApp.Pages
{
    public partial class CreditsPage : BaseContentPage<CreditsViewModel>
    {
        public CreditsPage()
        {
            InitializeComponent();

            var assembly = typeof(AuthApp.Pages.CreditsPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("AuthApp.Resources.Images.Credits.html");
            StreamReader reader = new StreamReader(stream);
            string htmlString = reader.ReadToEnd();

            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = htmlString;
            webView.Source = htmlSource;
        }
    }
}
