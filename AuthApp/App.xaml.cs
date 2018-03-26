using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Autofac;
using Autofac.Core;

using AuthApp.Pages;
using AuthApp.ViewModels;
using AuthApp.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AuthApp
{
    public partial class App : Application
    {
        public static IContainer Container { get; set; }

        public IAuthenticationService GetAuthenticationService => Container.Resolve<IAuthenticationService>();

        public App() : this(new IModule[] { })
        {
            
        }

        public App(IModule[] platformSpecificModules) 
        {
            BuildContainer(platformSpecificModules);

            InitializeComponent();

            MainPage = Container.Resolve<MainPage>();

        }

        /// <summary>
        /// Setup AutoFac Inversion of Control
        /// </summary>
        void BuildContainer(IModule[] platformSpecificModules)
        {
            var builder = new ContainerBuilder();

            // Add new registrations inside 'RegisterIoC' method, in the ContainerRegistration.cs file.
            ContainerRegistration.RegisterIoC(builder);

            RegisterPlatformSpecificModules(platformSpecificModules, builder);

            Container = builder.Build();
        }

        void RegisterPlatformSpecificModules(IModule[] platformSpecificModules, ContainerBuilder builder)
        {
            if(platformSpecificModules != null)
            {
                foreach (var platformSpecificModule in platformSpecificModules)
                {
                    builder.RegisterModule(platformSpecificModule);
                }
            }
        }

		protected override void OnStart()
		{
            // Handle when your app starts
		}

		protected override void OnSleep()
		{
            // Handle when your app sleeps
		}

		protected override void OnResume()
		{
            // Handle when your app resumes
		}
	}
}
