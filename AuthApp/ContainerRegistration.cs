using System;

using Autofac;

using AuthApp.ViewModels;
using AuthApp.Services;
using AuthApp.Pages;

using AuthApp.Services.Data;

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

            builder.RegisterType<TasksDataService>();
            builder.RegisterType<ProfileDataService>();

            // ViewModels
            builder.RegisterType<HomeViewModel>();
            builder.RegisterType<MasterViewModel>();
            builder.RegisterType<ApprovalsViewModel>();
            builder.RegisterType<SettingsViewModel>();
            builder.RegisterType<AssetManagementViewModel>();
            builder.RegisterType<SearchViewModel>();
            builder.RegisterType<ReportIssueViewModel>();
            builder.RegisterType<ProvideFeedbackViewModel>();
            builder.RegisterType<CreditsViewModel>();

        }
    }
}
