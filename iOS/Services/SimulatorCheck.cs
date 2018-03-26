using System;

using ObjCRuntime;

using AuthApp.Services;

namespace AuthApp.iOS.Services
{
    public class SimulatorCheck : ISimulatorCheck
    {
        public bool CheckIfSimulator()
        {
            return Runtime.Arch == Arch.SIMULATOR;
        }
    }
}
