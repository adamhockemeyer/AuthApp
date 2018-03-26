using System;

using Autofac;

using AuthApp.ViewModels;
using AuthApp.Services;
using AuthApp.Pages;

namespace AuthApp
{
    public static class ContainerRegistration
    {
        public static void RegisterIoC(ContainerBuilder builder)
        {
            // Views (Only ones which depend on constructor injection)
            builder.RegisterType<MainPage>();

            // Services
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<LoggerService>().As<ILoggerService>().SingleInstance();
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().SingleInstance();

            // ViewModels
            builder.RegisterType<HomeViewModel>();
            builder.RegisterType<MasterViewModel>();
            builder.RegisterType<TasksViewModel>();
            builder.RegisterType<SettingsViewModel>();
            builder.RegisterType<TaskEntryViewModel>();
            builder.RegisterType<SearchViewModel>();
            builder.RegisterType<ReportIssueViewModel>();

        }
    }
}
