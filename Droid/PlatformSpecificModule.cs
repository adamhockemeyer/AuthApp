using System;

using Autofac;

using AuthApp.Droid.Services;
using AuthApp.Services;

namespace AuthApp.Droid
{
    public class PlatformSpecificModule : Module
    {
		protected override void Load(ContainerBuilder builder)
		{
            base.Load(builder);

            // Register platform specific interface implementations here.
            // Example:
            // builder.RegisterType<LocalFilePathProvider>().As<ILocalFilePathProvider>();

            builder.RegisterType<SimulatorCheck>().As<ISimulatorCheck>().SingleInstance();
		}
	}
}
